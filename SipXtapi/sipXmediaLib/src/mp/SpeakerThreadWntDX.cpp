//
// Copyright (C) 2004-2006 SIPfoundry Inc.
// Licensed by SIPfoundry under the LGPL license.
//
// Copyright (C) 2004-2006 Pingtel Corp.  All rights reserved.
// Licensed to SIPfoundry under a Contributor Agreement.
//
// Copyright (C) 2006 ProfitFuel Inc.  All rights reserved.
// Licensed to SIPfoundry under a Contributor Agreement.
//  
// $$
///////////////////////////////////////////////////////////////////////////////

#ifdef USE_DIRECTX // [
// SYSTEM INCLUDES
#include <windows.h>
#include <process.h>
#include <mmsystem.h>
#include <dsound.h>
#include <assert.h>

// APPLICATION INCLUDES
#include "mp/dmaTask.h"
#include "mp/MpBufferMsg.h"
#include "mp/MpBuf.h"
#include "mp/MprToSpkr.h"
#include "mp/MpMediaTask.h"

// DEFINES
#define WM_ALT_HEARTBEAT    WM_USER+1

// EXTERNAL FUNCTIONS
extern void showWaveError(char *syscall, int e, int N, int line) ;  // dmaTaskWnt.cpp
extern GUID getDeviceGuid(UtlString deviceName, bool isCapture); //dmaTaskWnt.cpp

// EXTERNAL VARIABLES
extern HANDLE hMicThread;       // dmaTaskWnt.cpp
extern HANDLE hSpkrThread;      // dmaTaskWnt.cpp
extern int smSpkrQPreload;      // dmaTaskWnt.cpp
extern int frameCount;          // dmaTaskWnt.cpp

// CONSTANTS
// STATIC VARIABLE INITIALIZATIONS

// GLOBAL VARIABLE INITIALIZATIONS
struct AudioDevice {
  LPDIRECTSOUND8      DSound;
  LPDIRECTSOUNDBUFFER Buffer;
  DWORD               writeCursor;
  int                 sampleDelay;
};
static MpBufPtr gPrevBuf = NULL; // gPrevBuf is for future concealment use
static AudioDevice gAudioOut;
static AudioDevice gAudioOutCall;
static HANDLE gFrameCompletedEvent = CreateEvent(NULL, FALSE, FALSE, NULL); //Event signaled by DirectSound Notify

//These probably should be defines...
static DWORD gAudioFrameSize = ((N_SAMPLES * BITS_PER_SAMPLE) / 8);
static DWORD gAudioBufferSize = N_OUT_BUFFERS * gAudioFrameSize;

/* ============================ FUNCTIONS ================================= */

// Determine which speaker handle should be used
static AudioDevice* selectSpeakerDevice()
{
    AudioDevice* AudioOut;
    // If the ringer is not enabled we have a Call Audio Device 
    if (!DmaTask::isRingerEnabled() && gAudioOutCall.Buffer)
    {
        //Select output buffer and make sure it is the only one playing
        AudioOut = &gAudioOutCall;
    }
    // else the ringer is enabled or we don't have a call device, return
    // the default device
    else 
    {
        //Select output buffer and make sure it is the only one playing
        AudioOut = &gAudioOut;
    }
    return AudioOut;
}

// This callback will drive the media thread whether we have an audio
//device or not.
static void CALLBACK TimerCallbackProc(UINT wTimerID, UINT msg, DWORD dwUser, DWORD dw1, DWORD dw2) 
{    
  OsStatus res = MpMediaTask::signalFrameStart();
  switch (res) 
  {
      case OS_SUCCESS:
          frameCount++;
          break;
#ifdef DEBUG_WINDOZE /* [ */
      case OS_LIMIT_REACHED:
          // Should bump missed frame statistic
          osPrintf(" Frame %d: OS_LIMIT_REACHED\n", frameCount);
         break;
      case OS_WAIT_TIMEOUT:
          // Should bump missed frame statistic
          osPrintf(" Frame %d: OS_WAIT_TIMEOUT\n", frameCount);
          break;
      case OS_ALREADY_SIGNALED:
          // Should bump missed frame statistic
          osPrintf(" Frame %d: OS_ALREADY_SIGNALED\n", frameCount);
          break;
      default:
          osPrintf("Frame %d, signalFrameStart() returned %d\n",
                  frameCount, res);
          break;
#else /* DEBUG_WINDOZE ] [ */
      case OS_LIMIT_REACHED:
      case OS_WAIT_TIMEOUT:
      case OS_ALREADY_SIGNALED:
      default:
          // Should bump missed frame statistic
          break;
#endif /* DEBUG_WINDOZE ] */
                }
}

// This function will attempt to open a user specified audio device.
// If it fails, we will try to open any audio device that meets our requested format
// If we still fail, we will return 0, and display a message.
// TODO: Seems we have a problem when no audio device can be found. No calls can be made.
static int openAudioOut(LPGUID desiredDeviceGuid, AudioDevice* audioOut, 
                        int nSamplesPerFrame, int nChannels, 
                        int nSamplesPerSec, int nBitsPerSample)
{
   WAVEFORMATEX         fmt;
   DSBUFFERDESC         bufferDesc;
   LPDIRECTSOUNDBUFFER  primaryBuffer;
   DSBPOSITIONNOTIFY*   positionNotify;
   LPDIRECTSOUNDNOTIFY  buffPositionNotify;
   void*                bufferPtr;
   DWORD                lockSize;
   DWORD                playCursor;
   DWORD                writeCursor;
   HRESULT              res;
   
   //Zero out device struct.
   audioOut->DSound = NULL;
   audioOut->Buffer = NULL;
   audioOut->sampleDelay = 0;
   audioOut->writeCursor = 0;

   fmt.wFormatTag      = WAVE_FORMAT_PCM;
   fmt.nChannels       = nChannels;
   fmt.nSamplesPerSec  = nSamplesPerSec;
   fmt.nAvgBytesPerSec = (nChannels * nSamplesPerSec * nBitsPerSample) / 8;
   fmt.nBlockAlign     = (nChannels * nBitsPerSample) / 8;
   fmt.wBitsPerSample  = nBitsPerSample;
   fmt.cbSize          = 0;

   ZeroMemory(&bufferDesc, sizeof(DSBUFFERDESC));
   bufferDesc.dwSize  = sizeof(DSBUFFERDESC);

   res = DirectSoundCreate8(desiredDeviceGuid, &audioOut->DSound, NULL);
   if (res != DS_OK) {
     // Try and open the default device.  We should probably notify the user
     // that the default device wasn't selected.  
     res = DirectSoundCreate8(&DSDEVID_DefaultVoicePlayback, &audioOut->DSound, NULL);
   }
   if (res != DS_OK) {
     // hmm, couldn't open any audio device... things don't look good at this
     // point We need to work on showWaveError since it is built around 
     // waveOut functions.  Until then this shouldn't leave the debugger
     assert(false);
   }
   // We don't have access to the application window handle at this level.  
   // Maybe we can get it, but for now we will borrow from the mplayer method 
   // and use the Desktop window.  We are using DSSCL_PRIORITY to allow 16-bit
   // output.
   res = audioOut->DSound->SetCooperativeLevel(GetDesktopWindow(), DSSCL_PRIORITY);
   assert (res == DS_OK);
   //Get the primary buffer
   bufferDesc.dwFlags = DSBCAPS_PRIMARYBUFFER;
   bufferDesc.dwBufferBytes = 0;
   bufferDesc.lpwfxFormat = NULL;

   res = audioOut->DSound->CreateSoundBuffer(&bufferDesc, &primaryBuffer, NULL);
   assert (res == DS_OK);
   //Set the format of the primary buffer.  Would bumping the sampling rate
   //and letting windows resample provide a boost in quality?  How would it 
   //effect echo canceling?  Should we use a Full Duplex buffer later on and
   //use it for echo canceling, taking advantage of hardware where frequently
   //availible?
   res = primaryBuffer->SetFormat(&fmt);
   assert (res == DS_OK);
   primaryBuffer->Release();
   primaryBuffer = NULL;

   //Create the secondary buffer we will actually use.
   bufferDesc.dwFlags = DSBCAPS_CTRLPOSITIONNOTIFY |
                    DSBCAPS_GETCURRENTPOSITION2 |
                    DSBCAPS_LOCSOFTWARE |
                    DSBCAPS_GLOBALFOCUS;
   //We aren't going to queue the audio ourselves but put it all in the playback buffer
   //We will create enough space for N_OUT_BUFFERS of the framesize.
   bufferDesc.dwBufferBytes = gAudioBufferSize;
   bufferDesc.lpwfxFormat = &fmt;

   res = audioOut->DSound->CreateSoundBuffer(&bufferDesc, &audioOut->Buffer, NULL);
   assert (res == DS_OK);

   //Zero the audio buffer as we may need to call play before it is populated with audio.
   audioOut->Buffer->Lock(0, gAudioBufferSize, &bufferPtr, &lockSize, NULL, NULL, NULL);
   assert (lockSize == gAudioBufferSize);
   ZeroMemory(bufferPtr, lockSize);
   audioOut->Buffer->Unlock(bufferPtr, lockSize, NULL, NULL );

   //Setup notifies so we can drive more data.
   positionNotify = new DSBPOSITIONNOTIFY [N_OUT_BUFFERS];
   assert (positionNotify != NULL);  //This would happen if we are out of memory
   for (int i = 0; i < N_OUT_BUFFERS; i++)
   {
     //We will be notified at the end of each frame.
     positionNotify[i].dwOffset = gAudioFrameSize - 1 + i * gAudioFrameSize;
     positionNotify[i].hEventNotify = gFrameCompletedEvent;
   }

   res = audioOut->Buffer->QueryInterface(IID_IDirectSoundNotify8, (LPVOID *) &buffPositionNotify);
   assert (res == DS_OK);
   buffPositionNotify->SetNotificationPositions(N_OUT_BUFFERS, positionNotify);
   buffPositionNotify->Release();
   buffPositionNotify = NULL;

   //Calculate the delay between the play and write cursors.
   //We have to play at least 1 frame for this to work.
   audioOut->Buffer->Play(0,0,NULL);
   WaitForSingleObject(gFrameCompletedEvent, 100);
   WaitForSingleObject(gFrameCompletedEvent, 100);
   audioOut->Buffer->GetCurrentPosition(&playCursor, &writeCursor);
   audioOut->Buffer->Stop();
   audioOut->Buffer->SetCurrentPosition(0);
   audioOut->sampleDelay = writeCursor - playCursor;
   assert(audioOut->sampleDelay != 0);
   audioOut->writeCursor = ((audioOut->sampleDelay + gAudioFrameSize) / gAudioFrameSize) * gAudioFrameSize;
   return 1;
}


//Return number of frames skipped;
int advanceFrames(AudioDevice* audioOut,
                  MpBufPtr &buf)
{
    static int total = 0;
    int rv = 0;
    int framesToFlush = 0;
    DWORD safeWriteCursor;
    DWORD currentPlayCursor;
    //DWORD Status;
    DWORD bufferCursor;
    MpBufferMsg* pFlush;
    //audioOut->Buffer->GetStatus(&Status);
    audioOut->Buffer->GetCurrentPosition(&currentPlayCursor, &safeWriteCursor);
    bufferCursor = audioOut->writeCursor;
    if (currentPlayCursor == safeWriteCursor)
      return 0;
    //Advance write cursor if it isn't "safe"
    if (!(((safeWriteCursor <= audioOut->writeCursor) && 
         (safeWriteCursor + (gAudioBufferSize / 4) > audioOut->writeCursor)) ||
        ((safeWriteCursor > audioOut->writeCursor) && 
         (safeWriteCursor - (gAudioBufferSize * 3 / 4) > audioOut->writeCursor))))
      // Advance with at least smSpkrQPreload extra frames
      //Temporarily comment out smSpkrQPreload portion to reduce delay. May become
      //permanent.
    {
     // safeWriteCursor += gAudioFrameSize * 3;
      if (safeWriteCursor >= gAudioBufferSize)
        safeWriteCursor = 0;
      while (!(((safeWriteCursor <= audioOut->writeCursor) && 
         (safeWriteCursor + (gAudioBufferSize / 4) > audioOut->writeCursor)) ||
        ((safeWriteCursor > audioOut->writeCursor) && 
         (safeWriteCursor - (gAudioBufferSize * 3/ 4) > audioOut->writeCursor))))
      {
        rv++;
        audioOut->writeCursor += gAudioFrameSize;
        if (audioOut->writeCursor >= gAudioBufferSize) 
          audioOut->writeCursor = 0;        
      }
    }
    //assert (rv < 10);
    //if we have fallen behind, drain some of the spkr queue to avoid 
    //adding delay.  We won't drain it past SKIP_SPKR_BUFFERS.
    if (rv != 0 && MpMisc.pSpkQ)
    {
       if (rv > MpMisc.pSpkQ->numMsgs() - MprToSpkr::SKIP_SPKR_BUFFERS)
       {
          framesToFlush = MpMisc.pSpkQ->numMsgs() - MprToSpkr::SKIP_SPKR_BUFFERS;
       }
       else
       {
          framesToFlush = rv;
       }
       OsStatus  res;
       for (int i=0; i < framesToFlush; i++)
       {
          res = MpMisc.pSpkQ->receive((OsMsg*&) pFlush, OsTime::NO_WAIT);
          if (OS_SUCCESS == res) {
            if (buf)
              MpBuf_delRef(buf);
            buf = pFlush->getTag();
            gPrevBuf = buf;
            pFlush->releaseMsg();
          } 
       }
     }
     total += rv;
     //If things are working properly we shouldn't fall behind after everything stablizes.
     //Obviously we would ignore this in production.
     //This happens whenever anything else on the system is running.  Uncomment to debug.
     //assert(rv == 0 || frameCount < 1000);
     return rv;
}



void playBuffer(AudioDevice* audioOut,
                MpBufPtr mpBuffer)
{
    void* psDSLockedBuffer = NULL;
    DWORD lockedBytes = 0;
    DWORD playStatus = 0;

    audioOut->Buffer->Lock(audioOut->writeCursor, gAudioFrameSize,  
                          &psDSLockedBuffer, &lockedBytes, 
                          NULL, NULL, NULL);
    assert(lockedBytes == gAudioFrameSize);
    memcpy(psDSLockedBuffer, mpBuffer->pSamples, lockedBytes);
    audioOut->Buffer->Unlock(psDSLockedBuffer, lockedBytes, NULL, NULL);
    audioOut->Buffer->GetStatus(&playStatus);
    audioOut->writeCursor += gAudioFrameSize;
    if (audioOut->writeCursor >= gAudioBufferSize)
      audioOut->writeCursor = 0;
    if (!(playStatus & DSBSTATUS_LOOPING))
    {
      audioOut->Buffer->Play(0,0,DSBPLAY_LOOPING);
    }
}

//Return MpBuf to play when we are missing one.
static MpBufPtr conceal(MpBufPtr prev, int concealed)
{
#ifdef XXX_DEBUG_WINDOZE /* [ */
   MpBufPtr ret;
   Sample* src;
   Sample* dst;
   int len;
   int halfLen;
   int i;

   if (NULL == prev) {
      ret = MpBuf_getFgSilence();
      return ret;
   }
   len = MpBuf_getNumSamples(prev);
   ret = MpBuf_getBuf(MpBuf_getPool(prev), len, 0, MpBuf_getFormat(prev));
   src = MpBuf_getSamples(prev);
   dst = MpBuf_getSamples(ret);
   halfLen = (len + 1) >> 1;
   for (i=0; i<halfLen; i++) {
      dst[i] = src[len - i];
   }
   for (i=halfLen; i<len; i++) {
      dst[i] = src[i];
   }
   if (concealed > 2) {
      for (i=0; i<len; i++) {
         dst[i] = dst[i] >> 1; // attenuate
      }
   }
   return ret;
#else /* DEBUG_WINDOZE ] [ */
   return MpBuf_getFgSilence();
#endif /* DEBUG_WINDOZE ] */
}
//Get next MpBuf from SpkrQueue
static MpBufPtr GetNextFrame()
{
   MpBufferMsg* msg;
   MpBufferMsg* pFlush;
   MpBufPtr     ob;

   static int oPP = 0;
   static int concealed = 0; 

   static int flushes = 0;
   static int skip = 0;

   oPP++;

   //If the SpkQ is overfull drain it until it isn't
   while (MpMisc.pSpkQ && MprToSpkr::MAX_SPKR_BUFFERS < MpMisc.pSpkQ->numMsgs()) {
      OsStatus  res;
      flushes++;
      res = MpMisc.pSpkQ->receive((OsMsg*&) pFlush, OsTime::NO_WAIT);
      if (OS_SUCCESS == res) {
         MpBuf_delRef(pFlush->getTag());
         pFlush->releaseMsg();
      } else {
         osPrintf("DmaTask: queue was full, now empty (4)!"
            " (res=%d)\n", res);
      }
      if (flushes > 100) {
         osPrintf("outPrePrep(): %d playbacks, %d flushes\n", oPP, flushes);
         flushes = 0;
      }
   }
   //If our SpkQ doesn't have at least MIN_SPKR_BUFFERS we are going to skip 
   //pulling for SKIP_SPKR_BUFFERS frames to prelaod it.
   if (MpMisc.pSpkQ && (skip == 0) && (MprToSpkr::MIN_SPKR_BUFFERS > MpMisc.pSpkQ->numMsgs())) {
      skip = MprToSpkr::SKIP_SPKR_BUFFERS;
      assert(MprToSpkr::MAX_SPKR_BUFFERS >= skip);
#ifdef DEBUG_WINDOZE /* [ */
      osPrintf("Skip(%d,%d)\n", skip, oPP);
#endif /* DEBUG_WINDOZE ] */
   }

   ob = NULL;
   if (0 == skip) {
      if (MpMisc.pSpkQ && OS_SUCCESS == MpMisc.pSpkQ->receive((OsMsg*&)msg, OsTime::NO_WAIT)) {
         ob = msg->getTag();
         msg->releaseMsg();
      }
   } else {
      if (MpMisc.pSpkQ && MpMisc.pSpkQ->numMsgs() >= skip) skip = 0;
   }

   if (NULL == ob) {
      ob = conceal(gPrevBuf, concealed);
      concealed++;
   } else {
      concealed = 0;
      MpBuf_delRef(gPrevBuf);
      gPrevBuf = ob;
   }

   return ob;
}

static int openSpeakerDevices()
{
    LPDWORD       writeCursor;
    int           i;
    MpBufPtr      nextBuf;
    AudioDevice*  AudioOut;


    // If either the ringer or call device is set to NONE, don't engage any audio devices
    // This is pulled straight from the origional SpeakerThread.
    if ((stricmp(DmaTask::getRingDevice(), "NONE") == 0) || (stricmp(DmaTask::getCallDevice(), "NONE") == 0))
    {
        ResumeThread(hMicThread);
        return 1;
    }
    GUID RingDeviceGuid = getDeviceGuid(DmaTask::getRingDevice(), false);
    GUID CallDeviceGuid = getDeviceGuid(DmaTask::getCallDevice(), false);

    /*
     * Select in-call / ringer devices
     */
    if (RingDeviceGuid != GUID_NULL)
    {
      osPrintf("SpkrThread: Selected ring device: %s\n", DmaTask::getRingDevice());
    }

    if (CallDeviceGuid != GUID_NULL)
    {
      osPrintf("SpkrThread: Selected call device: %s\n", DmaTask::getCallDevice());
    }
    
    /*
     * Open ringer device
     */ 
    if (!openAudioOut(&RingDeviceGuid, &gAudioOut, N_SAMPLES, 1, SAMPLES_PER_SEC, BITS_PER_SAMPLE))
    {
        osPrintf("SpkrThread: Failed to open ring audio output channel\n\n");
        ResumeThread(hMicThread);
        return 1;
    }

    /*
     * Open in-call device
     */
    //If the in-call device is the same as the ringer don't allocate it, it 
    //selectSpeakerDevice() will use the ringer device.
    if (RingDeviceGuid != CallDeviceGuid)
    {
      if (!openAudioOut(&CallDeviceGuid, &gAudioOutCall, N_SAMPLES, 1, SAMPLES_PER_SEC, BITS_PER_SAMPLE))
      {
          osPrintf("SpkrThread: Failed to open call audio output channel\n\n");
          ResumeThread(hMicThread);
          return 1;
      }
    }
    else
    {
      //Clear the gAudioOutCall structure so selectSpeakerDevice won't return it.
      gAudioOutCall.Buffer = NULL;
      gAudioOutCall.DSound = NULL;
      gAudioOutCall.sampleDelay = 0;
      gAudioOutCall.writeCursor = 0;
    }

    AudioOut = selectSpeakerDevice();
    if (AudioOut->Buffer != NULL) 
    {

      // Pre load some data
      // for (i=0; i<smSpkrQPreload; i++) Change to count backwards.
      for (i=smSpkrQPreload; i > 0; i--)
      {            
          nextBuf=GetNextFrame(); //Should pass in PreLoad and return
                                  //Silence at begining if we don't have
                                  //Enough data to preload.
          if (nextBuf)
          {
            advanceFrames(AudioOut, nextBuf);
            playBuffer(AudioOut, nextBuf);
          }      
      }
    }
    return 0 ;
}

void closeSpeakerDevices()
{
  HRESULT res;
  // Clean up ringer audio
  if(gAudioOut.Buffer)
  {
    res = gAudioOut.Buffer->Stop();
    //assert (res == DS_OK);
    gAudioOut.Buffer->Release();
    gAudioOut.Buffer=NULL;
    gAudioOut.DSound->Release();
    gAudioOut.DSound=NULL;

  }
  if (gAudioOutCall.Buffer)
  {
    res = gAudioOutCall.Buffer->Stop();
    //assert (res == DS_OK);
    gAudioOutCall.Buffer->Release();
    gAudioOutCall.Buffer=NULL;
    gAudioOutCall.DSound->Release();
    gAudioOutCall.DSound=NULL;
  }
}

unsigned int __stdcall SpkrThread(LPVOID Unused)
{
    bool          bDone ;
    bool          LastRingerEnabled = false;
    static bool   bRunning = false ;
    int           MissedEvents = 0;
    int           MissedEventsCleared = 0;
    int           FramesPlayed = 0;
    int           oddDebug = 0;
    int           oddDebugSave = 0;
    int           resetFrame = 0;

    int           CurrentPlayFrame = 0;
    int           LastPlayFrameSave = 0;
    int           LastPlayFrame = 0;
    DWORD         CurrentPlayPosition = 0;
    DWORD         CurrentWritePosition = 0;
    AudioDevice*  AudioOut = NULL;
    AudioDevice*  LastAudioOut = NULL;
    MpBufPtr      buf;

    bool          frameSignaled = false;
    BOOL          bGotMsg = FALSE;
    MSG           tMsg;
    MMRESULT      timer = NULL;


    // Verify that only 1 instance of the SpeakerThread is running
    if (bRunning) 
    {
       ResumeThread(hMicThread);
       return 1 ;
    }
    else
    {
       bRunning = true ;
    }
    ResumeThread(hMicThread);
 
    openSpeakerDevices();
    //This timer will drive the media thread.
    timer = timeSetEvent(10, 0, TimerCallbackProc, GetCurrentThreadId(), TIME_PERIODIC);

    bDone = false ;
    while (!bDone)
    {
        
        frameSignaled = (0 == WaitForSingleObject(gFrameCompletedEvent, 100)); 
        if (frameSignaled) 
        { 
          MissedEventsCleared += MissedEvents;
          MissedEvents = 0;
        } else if (MissedEvents < 100) {
          MissedEvents++;
          frameSignaled=true;
        } 

        //Go ahead` and check for messages so we can quit.
        bGotMsg = PeekMessage(&tMsg, NULL, 0, 0, 1);
        if (bGotMsg && (tMsg.message == WM_QUIT || tMsg.message == WOM_CLOSE)) {
          frameSignaled = false;
          bDone = true;
          continue;
        }

        if (frameSignaled) 
        {                
                if (DmaTask::isOutputDeviceChanged())
                {                    
                    DmaTask::clearOutputDeviceChanged() ;
                    LastAudioOut = NULL;
                    closeSpeakerDevices();
                    openSpeakerDevices();
                    continue ;                    
                }

                //Stop last device if we should switch devices.  
                //PlayFrame will restart the correct buffer.
                if (LastRingerEnabled != DmaTask::isRingerEnabled() && LastAudioOut->Buffer)
                {
                  LastAudioOut->Buffer->Stop(); 
                  LastAudioOut->Buffer->SetCurrentPosition(0);
                  LastAudioOut->writeCursor = ((LastAudioOut->sampleDelay + gAudioFrameSize) / gAudioFrameSize) * gAudioFrameSize;
                  oddDebug = oddDebug | 2;
                  resetFrame = frameCount;
                }

                AudioOut = selectSpeakerDevice();
                if (AudioOut == LastAudioOut)
                {
                  AudioOut->Buffer->GetCurrentPosition(&CurrentPlayPosition, &CurrentWritePosition);
                  CurrentPlayFrame = CurrentPlayPosition / gAudioFrameSize;
                  if (CurrentPlayFrame >= LastPlayFrame)
                  {
                    FramesPlayed = CurrentPlayFrame - LastPlayFrame;
                  } else {
                    FramesPlayed = (gAudioBufferSize / gAudioFrameSize) - LastPlayFrame + CurrentPlayFrame;
                  } 
                  LastPlayFrameSave = LastPlayFrame;
                  LastPlayFrame = CurrentPlayFrame;
                  oddDebugSave = oddDebug;
                  oddDebug = oddDebug | 1 ;
                }
                else
                {                 
                  LastAudioOut = AudioOut;
                  AudioOut->Buffer->GetCurrentPosition(&CurrentPlayPosition, &CurrentWritePosition);
                  CurrentPlayFrame = CurrentPlayPosition / gAudioFrameSize;
                  FramesPlayed = 1;
                  LastPlayFrame = CurrentPlayFrame;
                  oddDebug = (oddDebug | 1) ^ 1;
                }
                if (FramesPlayed > 0)
                {
                  buf=GetNextFrame();
                  FramesPlayed -= advanceFrames(AudioOut, buf);
                  if (FramesPlayed > 0)
                  {
                    playBuffer(AudioOut, buf);
                    FramesPlayed--;
                  }
                  while (FramesPlayed > 0)
                  {
                    buf=GetNextFrame();
                    playBuffer(AudioOut, buf);
                    FramesPlayed--;
                  }
                }
                //assert (FramesPlayed >= -9);

        }
        else // !FrameSignaled
        {
            // Sky is falling, kick out so that we don't spin a high priority
            // thread.
            LastAudioOut = NULL;
            bDone = true ;
        }
      
        // record our last ringer state
        LastRingerEnabled = DmaTask::isRingerEnabled();
    }

    if (timer != NULL)
    {
      timeKillEvent(timer);
      timer = NULL;
    }
    closeSpeakerDevices() ;

    bRunning = false ;

    return 0;
}
#endif // USE_DIRECTX ]
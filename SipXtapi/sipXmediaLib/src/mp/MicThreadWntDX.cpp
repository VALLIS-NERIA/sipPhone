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
#include <dsound.h>
#include <mmsystem.h>
#include <assert.h>

// APPLICATION INCLUDES
#include "mp/dmaTask.h"
#include "mp/MpBufferMsg.h"
#include "mp/MpBuf.h"
#include "mp/MprToSpkr.h"
#include "mp/MpMediaTask.h"
#include "os/OsMsgPool.h"

// DEFINES
// EXTERNAL FUNCTIONS
extern void showWaveError(char *syscall, int e, int N, int line) ;  // dmaTaskWnt.cpp
extern GUID getDeviceGuid(UtlString deviceName, bool isCapture); //dmaTaskWnt.cpp

// EXTERNAL VARIABLES

extern HANDLE hMicThread;       // dmaTaskWnt.cpp
extern HANDLE hSpkrThread;      // dmaTaskWnt.cpp
extern int frameCount ;         // dmaTaskWnt.cpp

// CONSTANTS
// STATIC VARIABLE INITIALIZATIONS

// GLOBAL VARIABLE INITIALIZATIONS
static LPDIRECTSOUNDCAPTURE8 gMicIn;
static LPDIRECTSOUNDCAPTUREBUFFER gMicInBuffer;
static DWORD gMicInReadCursor;
static HANDLE gFrameCompletedEvent = CreateEvent(NULL, FALSE, FALSE, NULL); //Event signaled by DirectSound Notify
static OsMsgPool* DmaMsgPool = NULL;

//These maybe should be defines...
static DWORD gAudioFrameSize = ((N_SAMPLES * BITS_PER_SAMPLE) / 8);
static DWORD gAudioBufferSize = N_IN_BUFFERS * gAudioFrameSize;

/* ============================ FUNCTIONS ================================= */

int openAudioIn (LPGUID desiredDeviceGuid, 
                 LPDIRECTSOUNDCAPTURE8 &audioIn, 
                 LPDIRECTSOUNDCAPTUREBUFFER &audioInBuffer,
                 int nChannels,
                 int nSamplesPerSec,
                 int nBitsPerSample)
{
    HRESULT res;

    audioIn = NULL;
    audioInBuffer = NULL;
    DSCBUFFERDESC bufferDesc;
    WAVEFORMATEX fmt;
    DSBPOSITIONNOTIFY* positionNotify;
    LPDIRECTSOUNDNOTIFY buffPositionNotify;

        
    fmt.wFormatTag      = WAVE_FORMAT_PCM;
    fmt.nChannels       = nChannels;
    fmt.nSamplesPerSec  = nSamplesPerSec;
    fmt.nAvgBytesPerSec = (nChannels * nSamplesPerSec * nBitsPerSample) / 8;
    fmt.nBlockAlign     = (nChannels * nBitsPerSample) / 8;
    fmt.wBitsPerSample  = nBitsPerSample;
    fmt.cbSize          = 0;
    
    ZeroMemory(&bufferDesc, sizeof(DSCBUFFERDESC));
    bufferDesc.dwSize = sizeof(DSCBUFFERDESC);


    res = DirectSoundCaptureCreate8(desiredDeviceGuid, &audioIn, NULL);
    if (res != DS_OK) {
       // Try and open the default device.  We should probably notify the user
       // that the default device wasn't selected.  
       res = DirectSoundCaptureCreate8(&DSDEVID_DefaultVoiceCapture, &audioIn, NULL);
    }
    if (res != DS_OK) {
       // hmm, couldn't open any audio device... things don't look good at this
       // point We need to work on showWaveError since it is built around 
       // waveOut functions.  Until then this shouldn't leave the debugger
       assert(false);
    }

    bufferDesc.dwBufferBytes = gAudioBufferSize;
    bufferDesc.lpwfxFormat = &fmt;

    res = audioIn->CreateCaptureBuffer(&bufferDesc, &audioInBuffer, NULL);
    assert (res == DS_OK);

    //Setup notifies so we can read data.
    positionNotify = new DSBPOSITIONNOTIFY [N_IN_BUFFERS];
    assert (positionNotify != NULL);  //This would happen if we are out of memory
    for (int i = 0; i < N_IN_BUFFERS; i++)
    {
      //We will be notified at the mid point of each frame.
      positionNotify[i].dwOffset = i * gAudioFrameSize + gAudioFrameSize - 1;
      positionNotify[i].hEventNotify = gFrameCompletedEvent;
    }
    res = audioInBuffer->QueryInterface(IID_IDirectSoundNotify8, (LPVOID *) &buffPositionNotify);
    assert (res == DS_OK);
    res = buffPositionNotify->SetNotificationPositions(N_IN_BUFFERS, positionNotify);
    assert (res == DS_OK);
    buffPositionNotify->Release();
    buffPositionNotify = NULL;  
    
    return 1;
}

int openMicDevice(bool& bRunning)
{
  GUID MicDeviceGuid = getDeviceGuid(DmaTask::getMicDevice(), true);
  if (!openAudioIn(&MicDeviceGuid, gMicIn, gMicInBuffer, 1, SAMPLES_PER_SEC, BITS_PER_SAMPLE))
  {
    osPrintf("MicThread: Failed to open input device\n\n");
    ResumeThread(hSpkrThread);
    bRunning = false;
  }
  gMicInReadCursor = gAudioBufferSize - gAudioFrameSize;
  gMicInBuffer->Start(DSCBSTART_LOOPING);

  // Preload audio data isn't necessary with DSound.
  return 0 ;
}
bool SendFrame(MpBufPtr buf)
{
   static int     flushes = 0;
   MpBufferMsg*    pFlush = NULL;
   MpBufferMsg*   pMsg = (MpBufferMsg*) DmaMsgPool->findFreeMsg();
   if (NULL == pMsg)
     pMsg = new MpBufferMsg(MpBufferMsg::AUD_RECORDED);

   pMsg->setMsgSubType(MpBufferMsg::AUD_RECORDED);
 
   pMsg->setTag(buf);
   if (buf)
   {
     pMsg->setBuf(buf->pSamples);
     pMsg->setLen(buf->numSamples);
   }
   int MicQLen;
   if (MpMisc.pMicQ)
     MicQLen = MpMisc.pMicQ->numMsgs();
   if (MpMisc.pMicQ && MpMisc.pMicQ->numMsgs() >= MpMisc.pMicQ->maxMsgs())
   {
      // if its full, flush one and send
      OsStatus  res;
      flushes++;
      res = MpMisc.pMicQ->receive((OsMsg*&) pFlush, OsTime::NO_WAIT);
      if (OS_SUCCESS == res) {
         MpBuf_delRef(pFlush->getTag());
         pFlush->releaseMsg();
      } else {
         osPrintf("DmaTask: queue was full, now empty (3)!"
            " (res=%d)\n", res);
      }
   }
   if (MpMisc.pMicQ && OS_SUCCESS != MpMisc.pMicQ->send(*pMsg, OsTime::NO_WAIT))
   {
      MpBuf_delRef(buf);
   }
   if (!pMsg->isMsgReusable()) delete pMsg;
   return true;
}


int GetNextFrame(MpBufPtr &buf)
{
  int      framesSkipped =0;
  DWORD    recordCursor;
  DWORD    micCursor;
  DWORD    currentReadFrame;
  DWORD    maxAllowedReadFrame;
  void*    psDSLockedBuffer = NULL;
  DWORD    lockedBytes = 0;
  
  if (DmaTask::isMuteEnabled())
  {
    gMicInReadCursor += gAudioFrameSize;
    buf = MpBuf_getDmaSilence();
  }
  else
  {
    gMicInBuffer->GetCurrentPosition(&recordCursor, &micCursor);
    maxAllowedReadFrame = (micCursor / gAudioFrameSize);
    currentReadFrame = (gMicInReadCursor / gAudioFrameSize);
    //Test to see if we are reading safe data.
    if (!(((currentReadFrame < maxAllowedReadFrame) &&
        (currentReadFrame + gAudioBufferSize / 2 > maxAllowedReadFrame )) ||
        ((currentReadFrame > maxAllowedReadFrame) &&
        (currentReadFrame - gAudioBufferSize / 2 > maxAllowedReadFrame ))))
    { //We failed, lets advance(retreat) to the last frame we can safely read.
      framesSkipped++;
      gMicInReadCursor = (maxAllowedReadFrame - 1) * gAudioFrameSize;
      if (gMicInReadCursor >= gAudioBufferSize)
      {
        gMicInReadCursor = 0;
      }
    }
    buf = MpBuf_getBuf(MpMisc.UcbPool, N_SAMPLES, 0, MP_FMT_T12);
    gMicInBuffer->Lock(gMicInReadCursor, gAudioFrameSize, 
              &psDSLockedBuffer, &lockedBytes, NULL, NULL, NULL);
    assert (lockedBytes == gAudioFrameSize);
    memcpy(buf->pSamples, psDSLockedBuffer, lockedBytes);
    gMicInBuffer->Unlock(psDSLockedBuffer, lockedBytes, NULL, NULL);
    gMicInReadCursor += gAudioFrameSize;
    if (gMicInReadCursor >= gAudioBufferSize)
      gMicInReadCursor = 0;
  }
  //Not actually calculated yet.
  return framesSkipped;
}
void closeMicDevice()
{
  if (gMicInBuffer)
  {
    gMicInBuffer->Stop();
    gMicInBuffer->Release();
    gMicInBuffer=NULL;
    gMicIn->Release();
    gMicIn = NULL;
  }
}


unsigned int __stdcall MicThread(LPVOID Unused)
{
    int      recorded;
    bool     frameSignaled = false;
    MSG      tMsg;
    BOOL     bGotMsg ;
    MpBufPtr buf;
    bool     bDone ;
    int      missedEvents = 0;
    int      missedEventsCleared = 0;
    DWORD    recordCursor = 0;
    DWORD    readCursor = 0;
    int      recordFrame = -1;
    int      lastRecordFrame = -1;
    int      readFrame = -1;
    int      actualReadFrame = -1;
    int      lastActualReadFrame = -1;
    int      framesRecorded = 0;
    static bool bRunning = false ;

    // Verify that only 1 instance of the MicThread is running
    if (bRunning) 
    {
        ResumeThread(hSpkrThread);
        return 1 ;
    }
    else
    {
        bRunning = true ;
    }


    if (openMicDevice(bRunning))
    {
        return 1 ;
    }

    MpBufferMsg* pMsg = new MpBufferMsg(MpBufferMsg::AUD_RECORDED);
    DmaMsgPool = new OsMsgPool("DmaTask", (*(OsMsg*)pMsg),
            40, 60, 100, 5,
            OsMsgPool::SINGLE_CLIENT);

    // Start up Speaker thread
    ResumeThread(hSpkrThread);

#ifdef DEBUG_WINDOZE
    frameCount = 0;
#endif
    recorded = 0;
    bDone = false ;
    while (!bDone) 
    {
        buf = NULL;
        frameSignaled = (0 == WaitForSingleObject(gFrameCompletedEvent, 100)); 
        bGotMsg = PeekMessage(&tMsg, NULL, 0, 0, 1);
        if (bGotMsg && tMsg.message == WIM_CLOSE) 
        {
                bDone = true ;
                frameSignaled = false;
        } 
        if (frameSignaled) 
        { 
          missedEventsCleared += missedEvents;
          missedEvents = 0;
        } else if (missedEvents < 100) {
          missedEvents++;
          frameSignaled=true;
        } 
        if (frameSignaled)
        {
          if (DmaTask::isInputDeviceChanged())
          {                    
              DmaTask::clearInputDeviceChanged() ;
              closeMicDevice() ;
              lastRecordFrame = -1;
              openMicDevice(bRunning) ;
          }

          gMicInBuffer->GetCurrentPosition(&recordCursor, &readCursor);
          recordFrame = recordCursor / gAudioFrameSize;
          readFrame = readCursor / gAudioFrameSize - 1;
          if (readFrame < 0)
            readFrame = gAudioBufferSize / gAudioFrameSize - 1;
          actualReadFrame = gMicInReadCursor / gAudioFrameSize;
          if (actualReadFrame != readFrame)
          {
            gMicInReadCursor = readFrame * gAudioFrameSize;
            actualReadFrame = readFrame;
          }
          lastRecordFrame = recordFrame;

          if (actualReadFrame != lastActualReadFrame)
          {
            GetNextFrame(buf);
            if (!buf)
            {
              buf = MpBuf_getDmaSilence();
            }
            SendFrame(buf);
          }
          lastActualReadFrame = actualReadFrame;
        }
        else 
        {
            // Failed to get msg; don't spin high priority thread
            bDone = true ;
        }
    }
    closeMicDevice() ;    

    bRunning = false ;

    return 0;
}
#endif // USE_DIRECTX ]
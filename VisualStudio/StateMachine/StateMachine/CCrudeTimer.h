#ifndef _GAVI_CCRUDETIMER_HEADER_
#define _GAVI_CCRUDETIMER_HEADER_

#pragma comment(lib,"winmm.lib")

#include <windows.h>
#include "Singleton.h"

#define Clock CCrudeTimer::GetSingleton()

//!
class CCrudeTimer  : public Singleton<CCrudeTimer>
{
private:
  

  //set to the time (in seconds) when class is instantiated
  double m_dStartTime;

  //set the start time  
public:
	CCrudeTimer(){m_dStartTime = timeGetTime() * 0.001;}
	virtual ~CCrudeTimer(){}  

  //returns how much time has elapsed since the timer was started
  double GetCurrentTime(){return timeGetTime() * 0.001 - m_dStartTime;}

};



#endif
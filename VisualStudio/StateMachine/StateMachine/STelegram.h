#ifndef _GAVI_STELEGRAM_HEADER_
#define _GAVI_STELEGRAM_HEADER_

#include <iostream>
#include <math.h>
#include <windows.h>

struct STelegram
{
	char*	sSender;
	char*	sReceiver;

	int iMsg;
    
	double dDispatchTime;
	void* ExtraInfo;

	STelegram()
	{
		ZeroMemory(this,sizeof(STelegram));
	}
	STelegram(double time, char* sender, char* receiver, int msg, void* info = NULL) :
	dDispatchTime(time), sSender(sender), sReceiver(receiver), iMsg(msg), ExtraInfo(info)
	{}
};


///연산자 오버로딩을 한이유...
///직접적으로 사용하지는 않지만, STL set 을 사용하기 위해서 정의해 두었음..
///set 은 내부적으로 각각의 원소를 비교해서 같은 값은 추가 하지 않는데
///그 비교하는 작업때문에 == 나 < 같은 연산자를 오버로딩 하였다.
///이것을 정의해 두지 않으면 set 의 insert 를 사용시 에로사항이 꽃핌.

const double SmallestDelay = 0.25;

inline bool operator==(const STelegram& t1, const STelegram& t2)
{
  return ( fabs(t1.dDispatchTime-t2.dDispatchTime) < SmallestDelay) &&
          (t1.sSender == t2.sSender)        &&
          (t1.sReceiver == t2.sReceiver)    &&
          (t1.iMsg == t2.iMsg);
}


inline bool operator<(const STelegram& t1, const STelegram& t2)
{
  if (t1 == t2)
  {
    return false;
  }

  else
  {
    return  (t1.dDispatchTime < t2.dDispatchTime);
  }
}

#endif
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


///������ �����ε��� ������...
///���������� ��������� ������, STL set �� ����ϱ� ���ؼ� ������ �ξ���..
///set �� ���������� ������ ���Ҹ� ���ؼ� ���� ���� �߰� ���� �ʴµ�
///�� ���ϴ� �۾������� == �� < ���� �����ڸ� �����ε� �Ͽ���.
///�̰��� ������ ���� ������ set �� insert �� ���� ���λ����� ����.

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
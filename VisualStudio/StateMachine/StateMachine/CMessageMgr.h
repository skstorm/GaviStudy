#ifndef _GAVI_CMESSAGEMGR_HEADER_
#define _GAVI_CMESSAGEMGR_HEADER_

#include <set>
#include "STelegram.h"

using namespace std;

class CBaseGameEntity;

//! Entity 들간의 메시지를 주고 받는걸 관리한는 클래스
class CMessageMgr 
{
private :
	set<STelegram> m_PrioriryQ;
	void SendMsg(CBaseGameEntity* pReceiver, const STelegram& msg);
public :
	CMessageMgr(){}
	virtual ~CMessageMgr(){}
	
	void SendMsg(double delay, char* sender, char* receiver, int msg, void* ExtraInfo);
	void SendDelayedMsg();
};


#endif

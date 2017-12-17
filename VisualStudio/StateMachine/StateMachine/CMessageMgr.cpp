
#include "Singleton.h"

#include "CEntityMgr.h"
#include "CCrudeTimer.h"
#include "CBaseGameEntity.h"

#include "CMessageMgr.h"

void CMessageMgr::SendMsg(CBaseGameEntity* pReceiver, const STelegram& msg)
{
	pReceiver->HandleMessage(msg);
}

void CMessageMgr::SendMsg(double delay, char* sender, char* receiver, int msg, void* ExtraInfo)
{
	CBaseGameEntity* pSender = EntityMgr.GetEntityFromID(sender);
	CBaseGameEntity* pReceiver = EntityMgr.GetEntityFromID(receiver);

	if(pReceiver==NULL)
		return;

	STelegram telegram(0,sender,receiver,msg,ExtraInfo);

	if(delay<=0.0f)
	{
		SendMsg(pReceiver,telegram);
	}
	else
	{
		double CurrentTime = Clock.GetCurrentTime();
		telegram.dDispatchTime = CurrentTime + delay;
		m_PrioriryQ.insert(telegram);
	}
}

void CMessageMgr::SendDelayedMsg()
{
	double CurrentTime = Clock.GetCurrentTime();

	while(!m_PrioriryQ.empty() && (m_PrioriryQ.begin()->dDispatchTime < CurrentTime) && (m_PrioriryQ.begin()->dDispatchTime > 0))
	{
		const STelegram& telegram = *m_PrioriryQ.begin();
		CBaseGameEntity* pReceiver = EntityMgr.GetEntityFromID(telegram.sReceiver);

		SendMsg(pReceiver,telegram);

		m_PrioriryQ.erase(m_PrioriryQ.begin());
	}
}
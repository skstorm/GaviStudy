#include "CTaskState.h"
#include <iostream>
#include "CTask.h"

using namespace std;

/////////////// Mp3 Wait State/////////////////
CMp3TaskReady* CMp3TaskReady::Instance()
{
	static CMp3TaskReady instance;
	return &instance;
}

void CMp3TaskReady::Enter(CMp3Task* pTask)
{
	cout<<"CMp3TaskReady Enter~!!!"<<endl;
}

void CMp3TaskReady::Execute(CMp3Task* pTask)
{
}

void CMp3TaskReady::Exit(CMp3Task* pTask)
{
	cout<<"CMp3TaskReady Exit~!!!"<<endl;
}

void CMp3TaskReady::Proc(CMp3Task* pTask)
{
	cout<<"CMp3TaskReady Proc "<<pTask->m_iCount<<" !!!!!"<<endl;

	if(pTask->m_iCount >= 10)
	{
		pTask->m_pStateMachine->ChangeState(CMp3TaskRun::Instance());
	}
}

void CMp3TaskReady::Render(CMp3Task* pTask)
{
}

bool CMp3TaskReady::OnMessage(CMp3Task* pMp3Task, const STelegram& msg)
{
	return true;
}
///--------------------------------------------///

/////////////// Mp3 Wait State/////////////////
CMp3TaskRun* CMp3TaskRun::Instance()
{
	static CMp3TaskRun instance;
	return &instance;
}

void CMp3TaskRun::Enter(CMp3Task* pTask)
{
	cout<<"CMp3TaskRun Enter~!!"<<endl;
}

void CMp3TaskRun::Execute(CMp3Task* pTask)
{
}

void CMp3TaskRun::Exit(CMp3Task* pTask)
{
	cout<<"CMp3TaskRun Exit~!!"<<endl;
}

void CMp3TaskRun::Proc(CMp3Task* pTask)
{
	cout<<"CMp3TaskRun Proc "<<pTask->m_iCount<<" !!!!!"<<endl;
}

void CMp3TaskRun::Render(CMp3Task* pTask)
{
}

bool CMp3TaskRun::OnMessage(CMp3Task* pMp3Task, const STelegram& msg)
{
	return true;
}
///--------------------------------------------///
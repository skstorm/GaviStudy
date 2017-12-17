#include "CStateMachine.h"
#include "CTask.h"
#include "CTaskState.h"


CMp3Task::CMp3Task()
{
	m_pStateMachine = new CStateMachine<CMp3Task>(this);
	m_pStateMachine->SetCurrentState(CMp3TaskReady::Instance());
	m_pStateMachine->ChangeState(CMp3TaskReady::Instance());
}

CMp3Task::~CMp3Task()
{
	delete m_pStateMachine;
	m_pStateMachine = 0;
}

void CMp3Task::Update()
{
	++m_iCount;
	m_pStateMachine->Proc();
}
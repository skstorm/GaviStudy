#ifndef _CTASK_HEADER_
#define _CTASK_HEADER_

#include "CStateMachine.h"

class CTask
{
public :
	CTask() 
	{ 
			m_iCount =0;
	}
	virtual ~CTask(){}

	virtual void Update() = 0;

protected :
	int m_iCount;

};


class CMp3TaskReady;
class CMp3TaskRun;

class CMp3Task : public CTask
{
public :
	CMp3Task();
	virtual~ CMp3Task();

	void Update();

	friend class CMp3TaskReady;
	friend class CMp3TaskRun;

protected :
	CStateMachine<CMp3Task>* m_pStateMachine;
};

#endif

#ifndef _CTASKSTATE_HEADER_
#define _CTASKSTATE_HEADER_

#include "CState.h"

class CMp3Task;
struct STelegram;
/////////////// Mp3 Wait State/////////////////
class CMp3TaskReady : public CState<CMp3Task>
{	
public :
	
	static CMp3TaskReady* Instance();

	void Enter(CMp3Task* pMp3Task);
	void Execute(CMp3Task* pMp3Task);
	void Exit(CMp3Task* pMp3Task);

	void Render(CMp3Task* pMp3Task);
	void Proc(CMp3Task* pMp3Task);

	bool OnMessage(CMp3Task* pMp3Task, const STelegram& msg);

private :
	CMp3TaskReady(){}
	virtual ~CMp3TaskReady(){}
};
///--------------------------------------------///


/////////////// Mp3 Run State/////////////////
class CMp3TaskRun : public CState<CMp3Task>
{	
public :
	
	static CMp3TaskRun* Instance();

	void Enter(CMp3Task* pMp3Task);
	void Execute(CMp3Task* pMp3Task);
	void Exit(CMp3Task* pMp3Task);

	void Render(CMp3Task* pMp3Task);
	void Proc(CMp3Task* pMp3Task);

	bool OnMessage(CMp3Task* pMp3Task, const STelegram& msg);

private :
	CMp3TaskRun(){}
	virtual ~CMp3TaskRun(){}
};
///--------------------------------------------///

#endif

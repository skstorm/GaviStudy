#ifndef _GAVI_CSTATE_HEADER_
#define _GAVI_CSTATE_HEADER_
 
struct STelegram;

//! Base State Class
template <class Entity_Type>
class CState
{
protected :
public :
	CState(){}
	virtual ~CState(){}
	virtual void Enter(Entity_Type*) = 0;
	virtual void Execute(Entity_Type*) = 0;
	virtual void Exit(Entity_Type*) = 0;

	virtual void Render(Entity_Type*) {}
	virtual void Proc(Entity_Type*) {}

	virtual bool OnMessage(Entity_Type*, const STelegram&) = 0;

};

#endif
#ifndef _GAVI_CSTATEMACHINE_HEADER_
#define _GAVI_CSTATEMACHINE_HEADER_

#include "CState.h"
#include "STelegram.h"
#include "CMessageMgr.h"

//! StateMachine Class
template <class Entity_Type>
class CStateMachine
{
protected :
	Entity_Type*	m_pOwner;
	CState<Entity_Type>*	m_pCurrentState;
	CState<Entity_Type>*	m_pPreviousState;
	CState<Entity_Type>*	m_pGlobalState;

public :
	CStateMachine(Entity_Type* owner) :
	   m_pOwner(owner), m_pCurrentState(NULL), m_pPreviousState(NULL), m_pGlobalState(NULL) 
	   {}

	   virtual ~CStateMachine(){}

	   void SetCurrentState(CState<Entity_Type>* s) {m_pCurrentState = s;}	   
	   void SetPreviousState(CState<Entity_Type>* s) {m_pPreviousState = s;}
	   void SetGlobalState(CState<Entity_Type>* s) {m_pGlobalState = s;}

	   ///Render �� Proc �� ����鼭 ��� �ɰ� ����... ��а� ��ġ.
	   void Update()
	   {
		   if(m_pGlobalState)
			   m_pGlobalState->Execute(m_pOwner);
		   if(m_pCurrentState)
			   m_pCurrentState->Execute(m_pOwner);
	   }

	   void Render()
	   {
		   if(m_pGlobalState)
			   m_pGlobalState->Render(m_pOwner);
		   if(m_pCurrentState)
			   m_pCurrentState->Render(m_pOwner);
	   }

	   void Proc()
	   {
		   if(m_pGlobalState)
			   m_pGlobalState->Proc(m_pOwner);
		   if(m_pCurrentState)
			   m_pCurrentState->Proc(m_pOwner);
	   }

	   bool HandleMessage(const STelegram& msg) 
	   {
		   if(m_pCurrentState&&m_pCurrentState->OnMessage(m_pOwner,msg))
			   return true;

		   if(m_pGlobalState&&m_pGlobalState->OnMessage(m_pOwner,msg))
			   return true;

		   return false;
	   }

	   void ChangeState(CState<Entity_Type>* pNewState)
	   {
		   m_pPreviousState = m_pCurrentState;

		   m_pCurrentState->Exit(m_pOwner);

		   m_pCurrentState = pNewState;

		   m_pCurrentState->Enter(m_pOwner);
	   }

	   void ChangeState_Another(CState<Entity_Type>* pNewState) //���ҷ��� ���°� ������ ���¶���� ���¹ٲ��� ����.
	   {
		   if(pNewState==m_pCurrentState) 
			   return;

		   m_pPreviousState = m_pCurrentState;

		   m_pCurrentState->Exit(m_pOwner);

		   m_pCurrentState = pNewState;

		   m_pCurrentState->Enter(m_pOwner);
	   }

	   void RevertToPreviousState()
	   {
		   ChangeState(m_pPreviousState);
	   }

	   bool isInState(const CState<Entity_Type>& st) //������ ���¿� �Ű� ������ ���� ���°� ������ �˾ƺ��� �Լ�
	   {
		   if(typeid(*m_pCurrentState)==typeid(st)) return true;
		   return false;
	   }

	   CState<Entity_Type>* GetCurrentState() {return m_pCurrentState;}
	   CState<Entity_Type>* GetGlobalState() {return m_pGlobalState;}
	   CState<Entity_Type>* GetPreviousState() {return m_pPreviousState;}
};

#endif
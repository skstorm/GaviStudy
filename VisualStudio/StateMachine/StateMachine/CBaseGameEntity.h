#ifndef _GAVI_CBASEGAMEENTITY_HEADER_
#define _GAVI_CBASEGAMEENTITY_HEADER_

#include <iostream>
#include <stdio.h>


#pragma warning (disable:4805)

struct STelegram;

//! 
class CBaseEntity
{
protected :
	char* m_ID;	

	virtual void InitName()=0;

public :
	CBaseEntity()
	{
		m_ID = new char[20];
	}
	virtual ~CBaseEntity(){delete m_ID;}

	bool IsID(const char* id) 
	{
		if(strcmp(id,m_ID)==0) return true;
		else return false;
	}
	const char* GetID() {return m_ID;}
	void SetID(const char* id) { strcpy(m_ID,id); }

	virtual void Render() = 0;
	virtual void Proc(const float& timeDelta) = 0;

	virtual bool HandleMessage(const STelegram& msg) = 0;
};

//! 모든 오브젝트의 기본 Entity Class
class CBaseGameEntity : public CBaseEntity
{
protected :
	bool m_bTag;
	
public :
	CBaseGameEntity()
	{
		m_bTag = false; 
	}
	virtual ~CBaseGameEntity(){}

	bool IsTag() {return m_bTag;}
	void Tag() {m_bTag = true;}
	void UnTag() {m_bTag = false;}
};

#endif

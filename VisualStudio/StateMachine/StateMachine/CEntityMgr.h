#ifndef _GAVI_CENTITYMGR_HEADER_
#define _GAVI_CENTITYMGR_HEADER_

#include <map>
#include "Singleton.h"
#include "CBaseGameEntity.h"

#define  EntityMgr CEntityMgr::GetSingleton()

using namespace std;

//! EntityMgr Class
class CEntityMgr : public Singleton<CEntityMgr>
{
private :
	map<const char*,CBaseGameEntity*> m_EntityMap;

public :
	CEntityMgr(){}
	virtual ~CEntityMgr(){}

	void RegisterEntity(CBaseGameEntity* pNewEntity){m_EntityMap[pNewEntity->GetID()] = pNewEntity;}
	CBaseGameEntity* GetEntityFromID(const char* id){	return m_EntityMap[id];}
	void RemoveEntity(CBaseGameEntity* pEntity){ m_EntityMap.erase(m_EntityMap.find(pEntity->GetID())); }
};

#endif
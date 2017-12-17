//////////////////////////////////////////////////////////////////
//
//
//  Singleton Header File - Game Programming Gems 1
//
//  Data 07/03/12/��
//
//////////////////////////////////////////////////////////////////
#ifndef _SINGLETON_HEADER_
#define _SINGLETON_HEADER_

//////////////////////////////////////////////////////////////////
//  Singleton Class (Template) - ����ϴ� ������ �̱����� ��
//////////////////////////////////////////////////////////////////
template <class T>
class Singleton
{
private :
	static T* pSingleton;

public :
	Singleton()
	{
		int offset = (int)(T*)1 - (int)(Singleton <T>*)(T*)1;
		pSingleton = (T*)((int)this + offset);
	}

	~Singleton() { pSingleton = 0; }

	static T& GetSingleton()
	{
		if(!pSingleton)
		{
			pSingleton = new T;
		}
		else
		{
			return (*pSingleton);
		}
		return (*pSingleton);
	}

	static T* GetSingletonPtr() { return pSingleton; }
	static void DeleteSingleton() { delete pSingleton; pSingleton = 0; }
};

template <class T> T* Singleton<T>::pSingleton = 0;

#endif
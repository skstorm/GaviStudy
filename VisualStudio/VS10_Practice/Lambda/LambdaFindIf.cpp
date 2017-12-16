#include "LambdaTest.h"

#include <vector>
#include <algorithm>

using namespace std;

class User
{
public :
	User() : m_bDie(false) {}
	~User() {}

	void SetIndex(int index) { m_Index = index; }
	int GetIndex() {return m_Index; }
	void SetDie() { m_bDie = true; }
	bool IsDie() { return m_bDie; }

private :
	int m_Index;
	bool m_bDie;
};

void LambdaFindIf()
{
	gLamdaStart( "LambdaFindIf" );
	vector<User> Users;
	User tUser1; tUser1.SetIndex(1);  Users.push_back(tUser1);
	User tUser2; tUser2.SetIndex(2);  tUser2.SetDie(); Users.push_back(tUser2);
	User tUser3; tUser3.SetIndex(3);  Users.push_back(tUser3);
	User tUser4; tUser4.SetIndex(4);  tUser4.SetDie(); Users.push_back(tUser4);
	User tUser5; tUser5.SetIndex(5);  tUser5.SetDie(); Users.push_back(tUser5);
	

	vector<User> DiedUsers;
	
	//find_if 함수는 3번째의 펑터(또는 람다)의 반환값이
	//true일시 검색을 중단한다.
	find_if(Users.begin(), Users.end(), 
		[&DiedUsers](User& tUser)  -> bool
		{ 
			if(true ==tUser.IsDie())
			{
				DiedUsers.push_back(tUser);
			}
			return false;
		}
	 );
	 
	
	vector<User>::iterator Iter;
	Iter = find_if(Users.begin(), Users.end(),
		[] (User& tUser) -> bool 
		{
			return true == tUser.IsDie();
		}
	);
	cout<< "found Die User Index(Iter) : "<<Iter->GetIndex() <<endl;
	

	 for_each(DiedUsers.begin(), DiedUsers.end(), 
		 [](User& tUser)
		 {
			 cout<<"found Die User Index(vector) : "<<tUser.GetIndex()<<endl;
		 }
	 );

	gLamdaEnd();
}

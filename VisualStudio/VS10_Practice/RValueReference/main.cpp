#include <iostream>

using namespace std;

class NPC
{
public :
	int NPCCode;
	string Name;
	NPC() {cout<<"기본생성자"<<endl;}
	NPC(int _NpcCode, string _Name) {cout<<"인자있는 생성자"<<endl;}
	NPC(NPC& other) {cout<<"복사생성자"<<endl;}
	NPC& operator = (const NPC& npc) {cout <<"대입연산자"<<endl; ;return *this;}

	NPC(NPC&& other) {cout<<"Move 생성자"<<endl;}
	NPC& operator = (const NPC&& npc) {cout<<"Move 대입 연산자"<<endl; return *this; }
};

void main()
{
	//좌측 값과 우측값
	//좌측값은 라인이 지났어도 값유지
	//우측값은 라인을 벗어나면 값 없어짐...

	//대입값이 우측값이여야지 우측값참조 관련 연산자 발동./
	cout<<"1"<<endl;
	NPC npc1(NPC(10,"Orge1")); //인자, 무브

	cout<<endl<<"2"<<endl;
	NPC npc2(11,"Orge2");// 인자
	NPC npc3 = npc2;// 복사

	cout<<endl<<"3"<<endl;
	NPC npc4; NPC npc5; // 기본, 기본
	npc5 = npc4; //복사

	cout<<endl<<"4"<<endl;
	NPC npc6 = NPC(12,"Orge3"); //인자, 무브

	cout<<endl<<"5"<<endl;
	NPC npc7; NPC npc8; // 기본, 기본
	npc8 = std::move(npc7); //무브

	//주의!!! 우측값으로 쓰여진것은 다음에 쓸수있을지 없을지 알수 없다!!!
	//NPC npc9 = npc7; // <- 이런짓은 위험하니깐 하지말자...

	getchar();
}
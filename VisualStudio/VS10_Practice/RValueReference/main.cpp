#include <iostream>

using namespace std;

class NPC
{
public :
	int NPCCode;
	string Name;
	NPC() {cout<<"�⺻������"<<endl;}
	NPC(int _NpcCode, string _Name) {cout<<"�����ִ� ������"<<endl;}
	NPC(NPC& other) {cout<<"���������"<<endl;}
	NPC& operator = (const NPC& npc) {cout <<"���Կ�����"<<endl; ;return *this;}

	NPC(NPC&& other) {cout<<"Move ������"<<endl;}
	NPC& operator = (const NPC&& npc) {cout<<"Move ���� ������"<<endl; return *this; }
};

void main()
{
	//���� ���� ������
	//�������� ������ ����� ������
	//�������� ������ ����� �� ������...

	//���԰��� �������̿����� ���������� ���� ������ �ߵ�./
	cout<<"1"<<endl;
	NPC npc1(NPC(10,"Orge1")); //����, ����

	cout<<endl<<"2"<<endl;
	NPC npc2(11,"Orge2");// ����
	NPC npc3 = npc2;// ����

	cout<<endl<<"3"<<endl;
	NPC npc4; NPC npc5; // �⺻, �⺻
	npc5 = npc4; //����

	cout<<endl<<"4"<<endl;
	NPC npc6 = NPC(12,"Orge3"); //����, ����

	cout<<endl<<"5"<<endl;
	NPC npc7; NPC npc8; // �⺻, �⺻
	npc8 = std::move(npc7); //����

	//����!!! ���������� ���������� ������ ���������� ������ �˼� ����!!!
	//NPC npc9 = npc7; // <- �̷����� �����ϴϱ� ��������...

	getchar();
}
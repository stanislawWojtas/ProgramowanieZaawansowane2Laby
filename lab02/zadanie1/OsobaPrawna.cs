using System;

namespace TransakcjeBankowe{
    public class OsobaPrawna : PosiadaczRachunku{

        //dostęp przez gettery
        private string nazwa;
        private string siedziba;

        public string Nazwa{
            get{ return nazwa;}
        }
        public string Siedziba{
            get{ return siedziba;}
        }


        public OsobaPrawna(string nazwa, string siedziba){
            this.nazwa = nazwa;
            this.siedziba = siedziba;
        }

        public override string toString(){
            return "Osoba prawna: " + nazwa + " siedziba: " + siedziba;
        }
    }
}
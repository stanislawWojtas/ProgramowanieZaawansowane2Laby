using System;

namespace TransakcjeBankowe{
    public class OsobaFizyczna : PosiadaczRachunku{
        private string imie;
        private string nazwisko;
        private string drugieImie;
        private string pesel;
        private string numerPaszportu;

        public string Imie{
            get{ return imie;}
            set {imie = value;}
        }
        public string Nazwisko{
            get{ return nazwisko;}
            set {nazwisko = value;}
        }
        public string DrugieImie{
            get{ return drugieImie;}
            set {drugieImie = value;}
        }
        public string Pesel{
            get{ return pesel;}
            set {pesel = value;}
        }
        public string NumerPaszportu{
            get{ return numerPaszportu;}
            set {numerPaszportu = value;}
        }


        public OsobaFizyczna(string imie, string nazwisko, string drugieImie, string pesel, string numerPaszportu){
            
            if(pesel == null && numerPaszportu == null){
                throw new Exception("PESEL albo numer paszportu muszą być nie null");
            }

            this.imie = imie;
            this.nazwisko = nazwisko;
            this.drugieImie = drugieImie;
            this.pesel = pesel;
            this.numerPaszportu = numerPaszportu;
        }

        public override string toString(){
            return "Osoba fizyczna: " + imie + " " + nazwisko;
        }
    }
}
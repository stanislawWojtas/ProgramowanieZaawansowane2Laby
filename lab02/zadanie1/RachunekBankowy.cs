using System;
namespace TransakcjeBankowe{
    public class RachunekBankowy{

        private string numer;
        private decimal stanRachunku;
        private bool czyDozwolonyDebet;

        public string Numer{
            get{ return numer;}
        }
        public decimal StanRachunku{
            get{ return stanRachunku;}
        }
        public bool CzyDozwolonyDebet{
            get{ return czyDozwolonyDebet;}
        }

        
        List<PosiadaczRachunku> _PosiadaczeRachunku = new List<PosiadaczRachunku>();

        //lista transakcji
        List<Transakcja> _Transakcje = new List<Transakcja>();

        public RachunekBankowy(string numer, decimal stanRachunku, bool czyDozwolonyDebet, List<PosiadaczRachunku> posiadaczeRachunku){
            if(posiadaczeRachunku.Count < 1){
                throw new Exception("Rachunek musi mieć przynajmniej jednego posiadacza");
            }
            this.numer = numer;
            this.stanRachunku = stanRachunku;
            this.czyDozwolonyDebet = czyDozwolonyDebet;
            this._PosiadaczeRachunku = posiadaczeRachunku;
        }

        public static void DokonajTransakcji(RachunekBankowy rachunekZrodlowy, RachunekBankowy rachunekDocelowy, decimal kwota, string opis){
            
            if(kwota <= 0){
                throw new Exception("Kwota musi być większa od 0");
            }

            if(rachunekZrodlowy == null && rachunekDocelowy == null){ //jezeli oba rachunki sa null
                throw new Exception("Nie można dokonać transakcji");
            }


            if(rachunekZrodlowy != null && rachunekZrodlowy.CzyDozwolonyDebet == false && kwota > rachunekZrodlowy.StanRachunku){
                throw new Exception("Brak środków na rachunku źródłowym");
            }

            // Wpłata gotówkowa
            if(rachunekZrodlowy == null){
                rachunekDocelowy.stanRachunku += kwota;
                Transakcja transakcja = new Transakcja(null, rachunekDocelowy, kwota, opis);
                rachunekDocelowy._Transakcje.Add(transakcja);
            }

            // Wypłata gotówkowa
            else if(rachunekDocelowy == null){
                rachunekZrodlowy.stanRachunku -= kwota;
                Transakcja transakcja = new Transakcja(rachunekZrodlowy, null, kwota, opis);
                rachunekZrodlowy._Transakcje.Add(transakcja);
            }

            // Przelew
            else{
                rachunekZrodlowy.stanRachunku -= kwota;
                rachunekDocelowy.stanRachunku += kwota;
                Transakcja transakcja = new Transakcja(rachunekZrodlowy, rachunekDocelowy, kwota, opis);
                rachunekZrodlowy._Transakcje.Add(transakcja);
                rachunekDocelowy._Transakcje.Add(transakcja);
            }
        }
    }
}
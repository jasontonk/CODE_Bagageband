using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CODE_Bagageband.Model
{
    public class Aankomsthal : IObserver<Bagageband>
    {
        // TODO: Hier een ObservableCollection van maken, dan weten we wanneer er vluchten bij de wachtrij bij komen of afgaan.
        public ObservableCollection<Vlucht> WachtendeVluchten { get; private set; }
        public List<Bagageband> Bagagebanden { get; private set; }

        public Aankomsthal()
        {
            WachtendeVluchten = new ObservableCollection<Vlucht>();
            Bagagebanden = new List<Bagageband>();

            for(int x = 0; x < 3; x++)
            {
                Bagageband band = new Bagageband("Band " + x.ToString(), (30 + (x * 30)));
                band.Subscribe(this);
                Bagagebanden.Add(band);
            }
        }

        public void NieuweInkomendeVlucht(string vertrokkenVanuit, int aantalKoffers)
        {
            Vlucht vlucht = new Vlucht(vertrokkenVanuit, aantalKoffers);

            Bagageband legeBand = Bagagebanden.FirstOrDefault(b => b.AantalKoffers == 0);
            if (legeBand != null)
            {
                legeBand.HandelNieuweVluchtAf(vlucht);
            }
            else
            {
                WachtendeVluchten.Add(vlucht);
            }
        }

        /// <summary>
        /// Als er een update is wordt deze aangeroepen, je krijgt hier heel het object
        /// binnen. Dus elke keer als er een waarde binnen het object dat wij in de gaten houden
        /// verandert zal deze methode aangeroepen worden.We kunnen dan onze view aansturen dat
        /// de nieuwe waarde op het scherm moet komen.
        /// </summary>
        /// <param name="value"></param>
        public void OnNext(Bagageband value)
        {
            if (value.AantalKoffers == 0 && WachtendeVluchten.Any())
            {
                Vlucht volgendeVlucht = WachtendeVluchten.FirstOrDefault();
                WachtendeVluchten.RemoveAt(0);
                value.HandelNieuweVluchtAf(volgendeVlucht);
            }
        }

        /// <summary>
        /// Deze gaan we niet gebruiken
        /// </summary>
        public void OnError(Exception error)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Deze gaan we niet gebruiken
        /// </summary>
        public void OnCompleted()
        {
            throw new NotImplementedException();
        }
    }
}

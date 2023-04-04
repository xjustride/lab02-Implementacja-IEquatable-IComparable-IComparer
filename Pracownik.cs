using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_lab02_implementacjainterfejsów
{
	public class Pracownik
	{
		private string _nazwisko;

		public string Nazwisko
		{
			get { return _nazwisko; }
			set { _nazwisko = value.Trim(); }
		}

		private DateTime dataZatrudnienia;
		public DateTime DataZatrudnienia
		{
			get { return dataZatrudnienia; }
			set
			{
				if (value > DateTime.Now)
				{
					throw new ArgumentException();
				}
				dataZatrudnienia = value;
			}
		}


		private decimal _wyn;

		public decimal Wynagrodzenie
		{
			get => _wyn;
			set => _wyn = (value < 0) ? 0 : value;
			// {
			//     if (value < 0) _wyn = 0;
			//     else _wyn = value;
			// }
		}
		public override string ToString() => ($"{Nazwisko}, {DataZatrudnienia: dd MMM yyyy}, {Wynagrodzenie} PLN");
	}
}


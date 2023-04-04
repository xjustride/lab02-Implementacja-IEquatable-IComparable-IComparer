using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cs_lab02_implementacjainterfejsów
{
	public class Pracownik
	{
		public string Nazwisko
		{
			get { return Nazwisko; }
			set { Nazwisko = value.Trim(); }

		}

		DateTime DataZatrudnienia
		{
			get { return DataZatrudnienia; }
			set
			{
				if (DataZatrudnienia > DateTime.Now)
				{
					throw new ArgumentException();
				}
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


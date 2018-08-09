using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetGroup_ReservaSalas
{
    class ReservaSugestão
    {
        //  ATRIBUTOS
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int QuantidadePessoas { get; set; }
        public bool PossuiInternet { get; set; }
        public bool PossuiTvWebcam { get; set; }
        public int IdSala { get; set; }

        public ReservaSugestão(DateTime DataInicio, DateTime DataFim, int QuantidadePessoas, bool PossuiInternet, bool PossuiTvWebcam, int IdSala)
        {
            this.DataInicio = DataInicio;
            this.DataFim = DataFim;
            this.QuantidadePessoas = QuantidadePessoas;
            this.PossuiInternet = PossuiInternet;
            this.PossuiTvWebcam = PossuiTvWebcam;
            this.IdSala = IdSala;
        }
    }

    class Reserva
    {
        //  ATRIBUTOS
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
        public int QuantidadePessoas { get; set; }
        public bool PossuiInternet { get; set; }
        public bool PossuiTvWebcam { get; set; }
        
        //  Construtor
        public Reserva()
        {

        }

        public Reserva(DateTime _DataInicio, DateTime _DataFim, int _QuantidadePessoas, bool _PossuiInternet, bool _PossuiTvWebcam)
        {
            this.DataInicio = _DataInicio;
            this.DataFim = _DataFim;
            this.QuantidadePessoas = _QuantidadePessoas;
            this.PossuiInternet = _PossuiInternet;
            this.PossuiTvWebcam = _PossuiTvWebcam;
        }
    }
}

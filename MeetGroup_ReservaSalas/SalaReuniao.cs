using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MeetGroup_ReservaSalas
{
    class SalaReuniao
    {
        //  ATRIBUTOS
        public int IdSala { get; set; }
        public int Capacidade { get; set; }
        public bool PossuiInternet { get; set; }
        public bool PossuiTvWebcam { get; set; }
        public bool EstaReservada { get; set; }
        public List<Reserva> ReservasCadastradas;

        //  CONSTRUTOR

        public SalaReuniao()
        {
            this.ReservasCadastradas = new List<Reserva>();
        }


        public SalaReuniao(int idSala, int capacidade, bool possuiInternet, bool possuiTvWebcam)
        {
            this.IdSala = idSala;
            this.Capacidade = capacidade;
            this.PossuiInternet = possuiInternet;
            this.PossuiTvWebcam = possuiTvWebcam;
            this.EstaReservada = false;
            this.ReservasCadastradas = new List<Reserva>();
            //Console.WriteLine("cadastrada!");
        }

        //  MÉTODOS
        public void AgendarReserva(Reserva reserva)
        {
            this.ReservasCadastradas.Add(reserva);
        }

        public bool VerificarDisponibilidade(Reserva novaReserva)
        {
            bool retorno = true;

            if (this.ReservasCadastradas.Count == 0)
            {
                return retorno;
            } else
            {
                foreach (var reservaCadastrada in this.ReservasCadastradas)
                {
                    if ((novaReserva.DataInicio >= reservaCadastrada.DataInicio) && (novaReserva.DataInicio < reservaCadastrada.DataFim))
                        retorno = false;

                    if ((novaReserva.DataFim >= reservaCadastrada.DataInicio) && (novaReserva.DataFim < reservaCadastrada.DataFim))
                        retorno = false;
                }
            }

            return retorno;
        }
        /*{
             //Console.WriteLine("entrou em VerificarDisponibilidade()");
             if (this.ReservasCadastradas.Count == 0)
             {
                 return true;
             }
             else
             {
                 foreach (var reservaCadastrada in this.ReservasCadastradas)
                 {
                     //  OCORRE ANTES DA RESERVA CADASTRADA
                     if ((novaReserva.DataInicio < reservaCadastrada.DataInicio) && (novaReserva.DataFim <= reservaCadastrada.DataInicio))
                     {
                         return true;
                     }

                     //  OCORRE DEPOIS DA RESERVA CADASTRADA
                     if ((novaReserva.DataInicio >= reservaCadastrada.DataFim) && (novaReserva.DataFim > reservaCadastrada.DataFim))
                     {
                         return true;
                     }
                 }
             }

             return false;
         }*/

        public List<ReservaSugestão> VerificarSugestao(Reserva reserva)
        {
            // Cria uma lista para retorno
            var listaRetorno = new List<ReservaSugestão>();
            // Calcula o tempo de duranção da reserva a ser efetuada
            var timeSpanReserva = reserva.DataFim.Subtract(reserva.DataInicio);
            // Verifica se a sala possui todos os requetimentos de quantidade, intenet e tv
            if (!(this.Capacidade >= reserva.QuantidadePessoas) && (this.PossuiInternet == reserva.PossuiInternet) && (this.PossuiTvWebcam == reserva.PossuiTvWebcam))
            {
                // Caso não possua, retorna a lista de sugestões vazias
                return listaRetorno;
            }

            // Se possui todas as caracteristicas procura entre os horarias de cada reserva ja cadastrada se o tempo entre elas pode é maior que a a nova reserva
            // Se possui um intervalor entre as cadastradas que possa ser utilizado para a nova reserva
            for (int i = 0; i < this.ReservasCadastradas.Count() - 1; i++) // -1 no contador para não dar erro em this.ReservasCadastradas[i+1]
            {
                // Calcular a diferença de tempo entre a hora final da palesta e a procima palestra ( inicial[i+1] - final[i]                
                var timeSpanCadastrado = this.ReservasCadastradas[i + 1].DataInicio.Subtract(this.ReservasCadastradas[i].DataFim);
                // Verifica se o tempo da nova reserva é menor que o intervado entre as reservas
                if (timeSpanCadastrado >= timeSpanReserva)
                {
                    // Preenche dados 
                    DateTime dataInicial = this.ReservasCadastradas[i].DataFim;
                    DateTime dataFinal = dataInicial.Add(timeSpanReserva);
                    int quantidade = reserva.QuantidadePessoas;
                    bool tv = reserva.PossuiTvWebcam;
                    bool internet = reserva.PossuiInternet;
                    int idSala = this.IdSala;
                    // Adiciona sugestão para retorno
                    listaRetorno.Add(new ReservaSugestão(dataInicial, dataFinal, quantidade, internet, tv, idSala));
                }
            }
            return listaRetorno;
        }

        public void OrdenarLista()
        {
            this.ReservasCadastradas = this.ReservasCadastradas.OrderBy(o => o.DataInicio).ToList();
        }
    }
}

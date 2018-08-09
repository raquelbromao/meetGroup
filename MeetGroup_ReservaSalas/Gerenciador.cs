using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.IO;
using System.Globalization;

namespace MeetGroup_ReservaSalas
{

    class Gerenciador
    {

        const string arquivo = "json.json";
        private List<SalaReuniao> listaSalas = new List<SalaReuniao>();

        public Gerenciador()
        {
            this.listaSalas = new List<SalaReuniao>();
        }


        public void CadastrarSalas()
        {
            //  Cria objeto Salas e adiciona em uma List<>
            listaSalas.Add(new SalaReuniao(1, 10, true, true));
            listaSalas.Add(new SalaReuniao(2, 10, true, true));
            listaSalas.Add(new SalaReuniao(3, 10, true, true));
            listaSalas.Add(new SalaReuniao(4, 10, true, true));
            listaSalas.Add(new SalaReuniao(5, 10, true, true));

            listaSalas.Add(new SalaReuniao(6, 10, true, false));
            listaSalas.Add(new SalaReuniao(7, 10, true, false));

            listaSalas.Add(new SalaReuniao(8, 3, true, true));
            listaSalas.Add(new SalaReuniao(9, 3, true, true));
            listaSalas.Add(new SalaReuniao(10, 3, true, true));

            listaSalas.Add(new SalaReuniao(11, 20, false, false));
            listaSalas.Add(new SalaReuniao(12, 20, false, false));
        }

        public int VeririficarQuantidadeSalas()
        {
            return this.listaSalas.Count();
        }

        public bool SalvarArquivo()
        {
            try
            {
                // serialize JSON to a string and then write string to a file
                File.WriteAllText(arquivo, JsonConvert.SerializeObject(this.listaSalas));

                // serialize JSON directly to a file
                using (StreamWriter file = File.CreateText(arquivo))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    serializer.Serialize(file, this.listaSalas);
                }
                return true;
            }
            catch (Exception exc)
            {
                Console.WriteLine("Erro ao salvar arquivo:" + exc.Message);
                return false;
            }

        }

        public bool RestaurarArquivo()
        {
            try
            {
                // deserialize JSON directly from a file
                using (StreamReader file = File.OpenText(arquivo))
                {
                    JsonSerializer serializer = new JsonSerializer();
                    this.listaSalas = (List<SalaReuniao>)serializer.Deserialize(file, typeof(List<SalaReuniao>));
                    if (this.listaSalas == null)
                        this.listaSalas = new List<SalaReuniao>();
                    return true;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Erro ao restaurar arquivo:" + exc.Message);
                return false;
            }
        }


        public void SelecionarSugestões(Reserva reserva)
        {
            int pontoAnterior = 0, pontosPrioridade = 0, contador = 0, indice = -1;

            for (contador = 0; contador < listaSalas.Count; contador++)
            {
                pontosPrioridade = 0;
                Console.WriteLine("SALA -> {0}", listaSalas[contador].IdSala);

                if ((listaSalas[contador].PossuiTvWebcam == reserva.PossuiTvWebcam) == true)
                {
                    Console.WriteLine("\tPossui TvWebcam!");
                    pontosPrioridade += 1;
                }

                if ((listaSalas[contador].PossuiInternet == reserva.PossuiInternet) == true)
                {
                    Console.WriteLine("\tPossui Internet!");
                    pontosPrioridade += 1;
                }

                if (listaSalas[contador].Capacidade >= reserva.QuantidadePessoas)
                {
                    Console.WriteLine("\tComporta a quantidade de pessoas!");
                    pontosPrioridade += 3;
                }

                if (listaSalas[contador].VerificarDisponibilidade(reserva))
                {
                    Console.WriteLine("\tDisponível para reserva na data informada!");
                    pontosPrioridade += 5;
                }

                Console.WriteLine("\tPontos: {0}", pontosPrioridade);

                if (pontosPrioridade > pontoAnterior)
                {
                    pontoAnterior = pontosPrioridade;
                    indice = contador;
                }
            }

            Console.WriteLine("Sala escolhida -> {0}", indice);
        }

        public List<ReservaSugestão> AnalisarSugestao(Reserva reserva)
        {
            // inicia Sugestões para retorno
            var sugestoes = new List<ReservaSugestão>();
            // Para cada sala
            foreach (var sala in this.listaSalas)
            {
                // Verifica as sugestão e retorna uma lista
                var tempSugestoes = sala.VerificarSugestao(reserva);
                // Inisere as sugestoes para retorno
                foreach (var sugestao in tempSugestoes)
                {
                    // Adiciona as sugestões 
                    sugestoes.Add(sugestao);
                    // verifica se é maior ou igual a 3 e as retorna
                    if (sugestoes.Count >= 3)
                        return sugestoes;
                }
            }
            return sugestoes;
        }

        public void AnalisarReserva(Reserva reserva)
        {
            SalaReuniao salaEscolhida = new SalaReuniao();
            int contador = 0;
            bool salaEncontrada = false;

            while ((contador < listaSalas.Count) && (salaEncontrada == false))
            {
                if ((listaSalas[contador].VerificarDisponibilidade(reserva)) && (listaSalas[contador].Capacidade >= reserva.QuantidadePessoas) && (listaSalas[contador].PossuiInternet == reserva.PossuiInternet) && (listaSalas[contador].PossuiTvWebcam == reserva.PossuiTvWebcam))
                {
                    listaSalas[contador].AgendarReserva(reserva);
                    salaEncontrada = true;
                    salaEscolhida = listaSalas[contador];
                }
                contador++;
            }

            if (salaEncontrada == false)
            {
                Console.WriteLine("nenhuma sala encontrada");
                // SelecionarSugestões(reserva, salas);
            }
            else
            {
                Console.WriteLine("\nSala {0}", salaEscolhida.IdSala);
            }

            
        }


        public Reserva CriarReserva(string dados)
        {
            Reserva reserva = new Reserva();
            DateTime validacaoData1 = DateTime.Now.AddDays(1);
            DateTime validacaoData2 = DateTime.Now.AddDays(-40);
            //DateTime validacaoData2 = DateTime.Now.DayOfWeek
            string[] dadosReserva;

            dadosReserva = dados.Split(';');
            try
            {
                reserva.DataInicio = DateTime.ParseExact(dadosReserva[0] + " " + dadosReserva[1], "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);

            }
            catch (Exception exc)
            {
                Console.WriteLine("Erro ao converter data inicial: " + exc.Message);
                return null;
            }

            try
            {
                reserva.DataFim = DateTime.ParseExact(dadosReserva[2] + " " + dadosReserva[3], "dd-MM-yyyy HH:mm", CultureInfo.InvariantCulture);
            }
            catch (Exception exc)
            {

                Console.WriteLine("Erro ao converter data final: " + exc.Message);
                return null;
            }

            //  Validação de data para certificar que reserva não dura mais que 8horas
            if ((reserva.DataFim - reserva.DataInicio).TotalHours >= 8)
            {
                Console.WriteLine("Erro ao armazenar datas! Duração da reserva não pode durar mais que 8h!");
                return null;
            }

            //  Validação de data para certificar que data final é maior que data inicial
            if (reserva.DataFim <= reserva.DataInicio)
            {
                Console.WriteLine("Erro ao armazenar datas! Data final é igual ou menor que data inicial!");
                return null;
            }

            //  Validação de data para certificar que reserva está sendo feita 24h antes
            if (reserva.DataInicio <= validacaoData1)
            {
                Console.WriteLine("Erro ao armazenar datas! Data da reserva só pode ser feita com no mínimo 24h de antecedência!");
                return null;
            }

            //  Validação de data para certificar que reserva está sendo feita com no máximo 40 dias de antecedência
            if (reserva.DataInicio < validacaoData2)
            {
                Console.WriteLine("Erro ao armazenar datas! Data da reserva só pode ser feita com no máximo 40 dias de antecedência!");
                return null;
            }

            //  Validação de data da reserva para dias úteis
            if ((reserva.DataInicio.DayOfWeek == DayOfWeek.Saturday) || (reserva.DataInicio.DayOfWeek == DayOfWeek.Sunday))
            {
                Console.WriteLine("Erro ao armazenar datas! Data da reserva só pode ser feita em dias úteis!");
                return null;
            }

            try
            {
                reserva.QuantidadePessoas = Int32.Parse(dadosReserva[4]);
            }
            catch (Exception exc)
            {
                Console.WriteLine("Erro ao converter quantidade de pessoas: " + exc.Message);
                return null;
            }

            try
            {
                if (dadosReserva[5].ToUpper().Trim().CompareTo("SIM") == 0)
                    reserva.PossuiInternet = true;
                else if (dadosReserva[5].ToUpper().Trim().CompareTo("NÃO") == 0)
                    reserva.PossuiInternet = false;
                else
                {
                    Console.WriteLine("Erro ao converter flag de internet");
                    return null;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Erro ao converter flag de internet: " + exc.Message);
                return null;
            }

            try
            {
                if (dadosReserva[6].ToUpper().Trim().CompareTo("SIM") == 0)
                    reserva.PossuiTvWebcam = true;
                else if (dadosReserva[6].ToUpper().Trim().CompareTo("NÃO") == 0)
                    reserva.PossuiTvWebcam = false;
                else
                {
                    Console.WriteLine("Erro ao converter flag de tv/webcam");
                    return null;
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine("Erro ao converter flag de tv/webcam: " + exc.Message);
                return null;
            }

            return reserva;
        }

    }
}

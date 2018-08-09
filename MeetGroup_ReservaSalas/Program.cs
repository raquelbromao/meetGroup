using System;

namespace MeetGroup_ReservaSalas
{
    class Program
    {

        public static string entradaDadosReserva;

        static void Main(string[] args)
        {

            Gerenciador meetGroup = new Gerenciador();

            if (!meetGroup.RestaurarArquivo())
            {
                Console.WriteLine("Arquivo não pode ser restaurado, as salas serão criadas novamente!");
                meetGroup.CadastrarSalas();
                meetGroup.SalvarArquivo();
            }

            while (true)
            {

                //  Recebe e entrada dos dados da reserva e cria objeto Reserva
                Console.WriteLine("Por favor, informe os dados para a reserva da sala respeitando o seguinte formato:\n<DATA_INICIO>;<HORA_INICIO>;<DATA_FIM>;<HORA_FIM>;<QUANTIDADE_PESSOAS>;<ACESSO_INTERNET>;<TV_WEBCAM>\nExemplo: 26-02-2018;10:00;23-02-2018;12:00;10;Sim;Sim");
                entradaDadosReserva = Console.ReadLine();            
                Reserva reserva = meetGroup.CriarReserva(entradaDadosReserva);

                if (reserva != null)
                {
                    Console.WriteLine("RESERVA:\n- Data Início: {0}\n- Data Fim: {1}\n- Quantidade de Pessoas: {2}\n- Internet: {3}\n- Tv/Webcam: {4}", reserva.DataInicio, reserva.DataFim, reserva.QuantidadePessoas, reserva.PossuiInternet, reserva.PossuiTvWebcam);

                    // Validar e insere a reserva
                    meetGroup.AnalisarReserva(reserva);
                    // Salva a nova reserva no arquivo 
                    meetGroup.SalvarArquivo();
                }

                Console.ReadKey();

            }


            
        }
    }
}





//  Verifica se a reserva pode ser feita
// AnalisarReserva(reserva);

/*DateTime DataInicio1 = DateTime.ParseExact("26-06-1992 10:00", "dd-MM-yyyy hh:mm", CultureInfo.InvariantCulture);
DateTime DataFim1 = DateTime.ParseExact("26-06-1992 11:00", "dd-MM-yyyy hh:mm", CultureInfo.InvariantCulture);
DateTime DataInicio2 = DateTime.ParseExact("26-06-1992 09:00", "dd-MM-yyyy hh:mm", CultureInfo.InvariantCulture);
DateTime DataFim2 = DateTime.ParseExact("26-06-1992 10:00", "dd-MM-yyyy hh:mm", CultureInfo.InvariantCulture);

int result = DateTime.Compare(DataInicio2, DataInicio1);
Console.WriteLine("resultado = {0}", result);
Console.WriteLine(DataInicio2 < DataInicio1);
Console.WriteLine(DataFim2 <= DataInicio1);

if ((DataInicio2 < DataInicio1) && (DataFim2 <= DataInicio1))
{
    Console.WriteLine("reserva vem antes, pode reservar");
} */

/*Console.WriteLine("Por favor, informe os dados para a reserva da sala respeitando o seguinte formato:\n<DATA_INICIO>;<HORA_INICIO>;<DATA_FIM>;<HORA_FIM>;<QUANTIDADE_PESSOAS>;<ACESSO_INTERNET>;<TV_WEBCAM>\nExemplo: 26-02-2018;10:00;23-02-2018;12:00;10;Sim;Sim");
      entradaDadosReserva = Console.ReadLine();
      Console.WriteLine(entradaDadosReserva);

      dadosReserva = entradaDadosReserva.Split(';');
      dataInicio = dadosReserva[0].Split('-');
      horaInicio = dadosReserva[1];
      dataFim = dadosReserva[2].Split('-');
      horaFim = dadosReserva[3];
      quantidadePessoas = Int32.Parse(dadosReserva[4]);

      if (dadosReserva[5].ToUpper().Trim().CompareTo("SIM") == 0)
      {
          possuiInternet = true;
      } else
      {
          possuiInternet = false;
      }

      if (dadosReserva[6].ToUpper().Trim().CompareTo("SIM") == 0)
      {
          possuiTvWebcam = true;
      }
      else
      {
          possuiTvWebcam = false;
      }

      Console.WriteLine("RESERVA:\n- Data Início: {0}\n- Data Fim: {1}\n- Hora Início: {2}\n- Hora Fim: {3}\n- Quantidade de Pessoas: {4}\n- Internet: {5}\n- Tv/Webcam: {6}", dataInicio, dataFim, horaInicio, horaFim, quantidadePessoas, possuiInternet, possuiTvWebcam);
      Console.ReadKey();*/

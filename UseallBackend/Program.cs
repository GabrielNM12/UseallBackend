using System;
using UseallBackend.Context;
using Npgsql;

namespace UseallBackend
{
    class Program
    {
        public static string messageContinue {get; set;}
        static string nome {get; set;} = null!;
        static string cnpj {get; set;} = null!;
        static string endereco {get; set;} = null!;
        static string telefone {get; set;} = null!;

        static void Main(string[] args)
        {
            messageContinue = "Aperte Enter para continuar...";
            Fmenu();
        }

        static void Fmenu() {
            Console.WriteLine("---------------------------------------");
            Console.WriteLine("Menu SCRUM de Registro de Clientes");
            Console.WriteLine("Ações:\n" + 
                              "    1. Criar Registro;\n" +
                              "    2. Listar Clientes;\n" +
                              "    3. Buscar Cliente por Código;\n" +
                              "    4. Alterar Registro;\n" +
                              "    5. Apagar Registro;\n" +
                              "    0. Encerrar Aplicação;\n");
            Console.Write("Ação: ");
            int action = -1;
            try{ action = Int32.Parse(Console.ReadLine()); }
            catch (System.Exception) {}
            
            switch (action)
            {
                case 1:
                    Console.WriteLine("--------------------------");
                    NewReg();
                    Console.WriteLine(messageContinue);
                    Console.ReadLine();
                    Fmenu();
                break;
                case 2:
                    Console.WriteLine("--------------------------");
                    AllReg();
                    Console.WriteLine(messageContinue);
                    Console.ReadLine();
                    Fmenu();
                break;
                case 3:
                    Console.WriteLine("--------------------------");
                    FindByCodigo();
                    Console.WriteLine(messageContinue);
                    Console.ReadLine();
                    Fmenu();
                break;
                case 4:
                    Console.WriteLine("--------------------------");
                    AllReg();
                    ChangeReg();
                    Console.WriteLine(messageContinue);
                    Console.ReadLine();
                    Fmenu();
                break;
                case 5:
                    Console.WriteLine("--------------------------");
                    AllReg();
                    DeleteReg();
                    Console.WriteLine(messageContinue);
                    Console.ReadLine();
                    Fmenu();
                break;
                case 0:
                    Console.WriteLine("--------------------------");
                    Console.WriteLine("Encerrando Aplicação...");
                    Console.WriteLine("--------------------------");
                break;
                default:
                    Console.WriteLine("Escolha Inválida!");
                    Console.WriteLine(messageContinue);
                    Console.ReadLine();
                    Fmenu();
                break;
            }

        }

        static void NewReg() {

            string codigo = "1";
            codigo = LastCliCodigo();
            Console.Write("Nome: ");
            nome = Console.ReadLine();
            Console.Write("CNPJ: ");
            cnpj = Console.ReadLine();
            Console.Write("Endereco: ");
            endereco = Console.ReadLine();
            Console.Write("Telefone: ");
            telefone = Console.ReadLine();
            while(!ValidaCNPJ(cnpj)) {
                Console.WriteLine("Não é permitido o cadastro de CNPJ duplicado, informe outro cpnj");
                Console.Write("CNPJ: ");
                cnpj = Console.ReadLine();
            }
            string command = "INSERT INTO dbcadast.uscliente(codigo,nome,cnpj,dtcad,endereco,telefone) " + 
                             "VALUES("+ codigo + ",'"+ nome + "','"+ cnpj + "',CURRENT_DATE,'"+ endereco + "','"+ telefone + "')";

            var rdr = ApplicationDbContext.ReadCommand(command);
            Console.WriteLine("Cliente Incluído com Sucesso!");
        }

        static void AllReg() {
            var rdr = ApplicationDbContext.ReadCommand("SELECT * FROM dbcadast.uscliente ORDER BY codigo");
            string msg = String.Format( "Código | " + 
                                        "Nome | " + 
                                        "CNPJ | " + 
                                        "Data Cadastro | " + 
                                        "Endereço | " + 
                                        "Telefone");
            Console.WriteLine(msg);
            while (rdr.Read())
            {
                Console.WriteLine("--------------------------");
                msg = String.Format( rdr.GetInt32(0) + " | " +
                                     rdr.GetString(1).Trim() + " | " +
                                     rdr.GetString(2).Trim() + " | " +
                                     rdr[3].ToString().Trim() + " | " +
                                     rdr.GetString(4).Trim() + " | " +
                                     rdr.GetString(5).Trim());
                Console.WriteLine(msg);
            }
        }

        static void FindByCodigo() {
            Console.Write("Informe o código do cliente: ");
            string codigo = Console.ReadLine();
            if (codigo == "") codigo = "0"; 
            var rdr = ApplicationDbContext.ReadCommand("SELECT * FROM dbcadast.uscliente WHERE codigo="+codigo);
            string msg = String.Format( "Código | " + 
                                        "Nome | " + 
                                        "CNPJ | " + 
                                        "Data Cadastro | " + 
                                        "Endereço | " + 
                                        "Telefone");
            if (!rdr.HasRows) {
                Console.WriteLine("Cliente não encontrado...");
                return;
            }
            Console.WriteLine(msg);
            while (rdr.Read())
            {
                Console.WriteLine("--------------------------");
                msg = String.Format( rdr.GetInt32(0) + " | " +
                                     rdr.GetString(1).Trim() + " | " +
                                     rdr.GetString(2).Trim() + " | " +
                                     rdr[3].ToString().Trim() + " | " +
                                     rdr.GetString(4).Trim() + " | " +
                                     rdr.GetString(5).Trim());
                Console.WriteLine(msg);
            }
        }

        static void ChangeReg() {
            Console.Write("Informe o código do cliente: ");
            string codigo = Console.ReadLine();
            if (codigo == "") codigo = "0"; 
            var rdr = ApplicationDbContext.ReadCommand("SELECT * FROM dbcadast.uscliente WHERE codigo="+codigo);
            if (!rdr.HasRows) {
                Console.WriteLine("Cliente não encontrado...");
                return;
            }
            while (rdr.Read())
            {
                Console.WriteLine("--------------------------");
                Console.Write("Nome Anterior: " + rdr.GetString(1).Trim() + " | Nome Atual: ");
                nome = Console.ReadLine();
                Console.Write("CNPJ Anterior: " + rdr.GetString(2).Trim() + " | CNPJ Atual: ");
                cnpj = Console.ReadLine();
                while(!ValidaCNPJ(cnpj) && cnpj != rdr.GetString(2).Trim()) {
                    Console.WriteLine("Não é permitido o cadastro de CNPJ duplicado, informe outro cpnj");
                    Console.Write("CNPJ Anterior: " + rdr.GetString(2).Trim() + " | CNPJ Atual: ");
                    cnpj = Console.ReadLine();
                }
                Console.Write("Endereço Anterior: " + rdr.GetString(4).Trim() + " | Endereço Atual: ");
                endereco = Console.ReadLine();
                Console.Write("Telefone Anterior: " + rdr.GetString(5).Trim() + " | Telefone Atual: ");
                telefone = Console.ReadLine();
            }
            string command = "UPDATE dbcadast.uscliente" +
                             " SET codigo='"+codigo+"',nome='"+nome+"',cnpj='"+cnpj+"',endereco='"+endereco+"',telefone='"+telefone +"'"+
                             " WHERE codigo=" + codigo;
            int rowsAffected = ApplicationDbContext.RowsAffected(command);
            rdr = ApplicationDbContext.ReadCommand(command);
            if (rowsAffected <= 0) {
                Console.WriteLine("Cliente não encontrado...");
                return;
            }
            Console.WriteLine("Cliente alterado com sucesso.");
        }

        static void DeleteReg() {
            Console.Write("Informe o código do cliente: ");
            string codigo = Console.ReadLine();
            if (codigo == "") codigo = "0"; 
            string command = "DELETE FROM dbcadast.uscliente WHERE codigo="+codigo;
            int rowsAffected = ApplicationDbContext.RowsAffected(command);
            var rdr = ApplicationDbContext.ReadCommand(command);
            if (rowsAffected <= 0) {
                Console.WriteLine("Cliente não encontrado...");
                return;
            }
            Console.WriteLine("Cliente deletado com sucesso.");
        }

        static string LastCliCodigo() {
            var codigoR = ApplicationDbContext.ReadCommand("SELECT codigo FROM dbcadast.uscliente WHERE codigo=(SELECT max(codigo) FROM dbcadast.uscliente)");
            if (codigoR.Read()) {
                try
                {
                    return (codigoR.GetInt32(0) + 1).ToString();
                }
                catch (System.Exception)
                {
                }
            }
            return "1";
        }

        static bool ValidaCNPJ(string cnpj) {
            var rdr = ApplicationDbContext.ReadCommand("SELECT cnpj FROM dbcadast.uscliente WHERE cnpj='"+cnpj+"'");
            return !rdr.HasRows;
        }
    }
}

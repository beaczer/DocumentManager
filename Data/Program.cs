using LastChance.DAL;
using LastChance.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var ctx = new DocumentContext())
            {
                var xw = ctx.Operations.Where(x => x.IdLine==1).ToList();
                foreach (var item in xw)
                {
                    Console.WriteLine("{0} {1} {2} {3}",item.IdLine, item.IdOperation,item.MachineNumber,item.OperationNumber);
                }

                //Line Linia1 = new Line() { IdLine = 1, LineName = "Montaż Silnika" };
                //Line Linia2 = new Line() { IdLine = 2, LineName = "Montaż Sensora" };
                //Line Linia3 = new Line() { IdLine = 3, LineName = "Obróbka obudowy " };
                //Line Linia4 = new Line() { IdLine = 4, LineName = "Czyszczenie obudowy  " };
                //Line Linia5 = new Line() { IdLine = 5, LineName = "Montaż finalny" };
                //Line Linia6 = new Line() { IdLine = 6, LineName = "Magazyn" };

                //Operation op1 = new Operation() { IdLine = 1, IdOperation = 1, MachineNumber = 100,  OperationNumber=10 };
                //Operation op2 = new Operation() { IdLine = 1, IdOperation = 2, MachineNumber = 101, OperationNumber = 20 };
                //Operation op3 = new Operation() { IdLine = 1, IdOperation = 3, MachineNumber = 102, OperationNumber = 30 };
                //Operation op4 = new Operation() { IdLine = 1, IdOperation = 4, MachineNumber = 103, OperationNumber = 40 };

                //Operation op5 = new Operation() { IdLine = 2, IdOperation = 5, MachineNumber = 104, OperationNumber = 50 };
                //Operation op6 = new Operation() { IdLine = 2, IdOperation = 6, MachineNumber = 105, OperationNumber = 60 };
                //Operation op7 = new Operation() { IdLine = 2, IdOperation = 7, MachineNumber = 106, OperationNumber = 70 };
                //Operation op8 = new Operation() { IdLine = 2, IdOperation = 8, MachineNumber = 107, OperationNumber = 80 };

                //ctx.Lines.Add(Linia1);
                //ctx.Lines.Add(Linia2);
                //ctx.Lines.Add(Linia3);
                //ctx.Lines.Add(Linia4);
                //ctx.Lines.Add(Linia5);
                //ctx.Lines.Add(Linia6);
                //ctx.Operations.Add(op1);
                //ctx.Operations.Add(op2);
                //ctx.Operations.Add(op3);
                //ctx.Operations.Add(op4);
                //ctx.Operations.Add(op5);
                //ctx.Operations.Add(op6);
                //ctx.Operations.Add(op7);
                //ctx.Operations.Add(op8);
                //ctx.SaveChanges();
            }
        }
    }
}

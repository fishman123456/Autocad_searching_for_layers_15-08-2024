using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.EditorInput;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Application = Autodesk.AutoCAD.ApplicationServices.Application;

namespace Autocad_searching_for_layers_15_08_2024
{
    // класс для проверки текущей даты
    public static class CheckDateWork
    {
        public static void CheckDate()
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            DateTime dt1 = DateTime.Now;
            DateTime dt2 = DateTime.Parse("26/10/2025");
            Application.ShowAlertDialog("работаем до ...");

            if (dt1.Date > dt2.Date)
            {
                Application.ShowAlertDialog("Your Application is Expire");
                // Выход из проложения добавил 01-01-2024. Чтобы порядок был....
                Application.ShowAlertDialog("1 Save your drawings !!!");
                Application.ShowAlertDialog("2 Save your drawings !!!");
                Application.ShowAlertDialog("3 Save your drawings !!!");
                Application.ShowAlertDialog("Autocad Process Kill !!!");
                // закрытие процесса autocad 09-01-2024
                foreach (Process Proc in Process.GetProcesses())
                {
                    if (Proc.ProcessName.Equals("acad"))
                    {
                        Proc.CloseMainWindow();
                        Proc.Kill();
                    }
                }
                //w1.Close();
            }
            else
            {
                //MessageBox.Show("Работайте до   " + dt2.ToString());
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autodesk.AutoCAD;
using Autodesk.AutoCAD.Runtime;
using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Geometry;
using Autodesk.AutoCAD.GraphicsInterface;

namespace Autocad_searching_for_layers_15_08_2024
{
    public class ListSeachLayers
    {
        [CommandMethod("U_83_ListLayers", CommandFlags.Redraw)]
        public void ListLay()
        {



            // делаем видимым обьект окно в текущем пространстве 
            UserControl1 windowSeach;
            // проверка по дате
            CheckDateWork.CheckDate();

            // проверяем на существования окна
            if (WinCloseTwo.countWin == 0)
            {
                windowSeach = new UserControl1();
                windowSeach.Show();
                windowSeach.Activate();
                WinCloseTwo.countWin = 1;
            }
            Editor ed = Application.DocumentManager.MdiActiveDocument.Editor;
            Database db = HostApplicationServices.WorkingDatabase;
            var doc = Application.DocumentManager.MdiActiveDocument;
            Transaction tr = db.TransactionManager.StartTransaction();
            // выбираем все обьекты на чертеже
            TypedValue[] tvs = new TypedValue[2] {
                new TypedValue((int)DxfCode.Start, "INSERT"),
                new TypedValue((int)DxfCode.LayerName) };
            // фильтр выбора

            SelectionFilter sf = new SelectionFilter(tvs);

            PromptSelectionResult res = ed.SelectAll();
            // создаём массив всех обьектов, потом будем проверять по совпадению имени слоя
            ObjectId[] idArray = new ObjectId[2];

            SelectionSet selSet = res.Value;
            // проверка на пустоту чертежа
            if (selSet != null)
            {
                idArray = selSet.GetObjectIds();
            }
            else
            {
                Application.ShowAlertDialog("нет обьекта в чертеже");
                return;
            }
            // создаем список обьектов, потом в массив преобразуем и выделим в модели 24-07-2024
            List<Polyline3d> pid = new List<Polyline3d>();
            List<ObjectId> idPoly = new List<ObjectId>();
            // считываем таблицу слоев
            LayerTable tblLayer = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead, false);
            // количество кабелей(3D polyline)
            int countLay = 0;
            // количество совпадений с тексбоксом
            int countCab = 0;
            

            BlockTable bt = tr.GetObject(db.BlockTableId, OpenMode.ForRead) as BlockTable;
            BlockTableRecord btr = tr.GetObject(bt[BlockTableRecord.ModelSpace], OpenMode.ForRead) as BlockTableRecord;
            // ищем по типу 3d полилинии и добавляем их в массив
            foreach (ObjectId id in btr)
            {
                if (id.ObjectClass.Name == RXClass.GetClass(typeof(Polyline3d)).Name)
                {
                    Polyline3d polyline3D = tr.GetObject(id, OpenMode.ForRead) as Polyline3d;
                    pid.Add(polyline3D);
                    idPoly.Add(id);
                    
                    countLay++;
                   
                }
            }

            List<ObjectId> idPolylyne = new List<ObjectId>();
            foreach (Polyline3d varible in pid)
            {
                ed.WriteMessage(varible.Layer + " - ");
                ed.WriteMessage(varible.Length + "\n");
                // проходим по всем строкам из текстбокса для проверки на совпадения
                for (int i = 0; i < WinCloseTwo.massSeach.Length; i++)
                {
                    // условия по совпадению с текстбоксом
                    if(varible.Layer == WinCloseTwo.massSeach[i])
                    {
                        idPolylyne.Add(varible.Id);
                        // выбор в модели 3d полилиний     
                        countCab++;
                    }
                }
                // выбор в модели 3d полилиний     
                SelectionSet ss1 = SelectionSet.FromObjectIds(idPolylyne.ToArray());
                ed.SetImpliedSelection(ss1.GetObjectIds());
            }
            // разность сущ - искомые
            int eqelsCab = countLay - countCab;
            ed.WriteMessage("количество кабеля всего    = " + countLay + "\n");
            ed.WriteMessage("количество совпадений      = " + countCab + "\n");
            ed.WriteMessage("разность: сущ - тексбокс   = " + eqelsCab + "\n");
            ed.WriteMessage("поиск завершен" + DateTime.Now.ToString() + "\n");
        }
    }
}

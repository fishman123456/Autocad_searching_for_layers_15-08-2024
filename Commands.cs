using Autodesk.AutoCAD.ApplicationServices;
using Autodesk.AutoCAD.DatabaseServices;
using Autodesk.AutoCAD.EditorInput;
using Autodesk.AutoCAD.Runtime;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Autocad_searching_for_layers_15_08_2024
{
    // программа для поиска слоёв в чертеже 15-08-2024
    public class Commands
    {
        [CommandMethod("U_83_searching_for_layers")]

        static public void EntitiesOnLayer()
        {
            // проверяем по дате
            CheckDateWork.CheckDate();
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            // вводите имя слоя по которому ищем
            PromptResult pr = ed.GetString("\nEnter name of layer: ");
            List<ObjectId> listSaveObjectUser = new List<ObjectId>();
            if (pr.Status == PromptStatus.OK)
            {
                ObjectIdCollection ents = GetEntitiesOnLayer(pr.StringResult);
            
                ed.WriteMessage(
                  "\nНайдено {0} обьектов {1} на слое {2}",
                  ents.Count, (ents.Count == 1 ? "y" : "ies"),
                  pr.StringResult
                );
            }
        }
        private static ObjectIdCollection
          GetEntitiesOnLayer(string layerName)
        {
            Document doc = Application.DocumentManager.MdiActiveDocument;
            Editor ed = doc.Editor;
            Database db = doc.Database;

            // список для сохранения выбранных обьектов
            List<ObjectId> listSaveObject = new List<ObjectId>();

            // Build a filter list so that only entities
            // on the specified layer are selected
            TypedValue[] tvs = new TypedValue[1] {
             new TypedValue((int)DxfCode.LayerName, layerName)
                };
            // фильтр выбора
            SelectionFilter sf = new SelectionFilter(tvs);

            // транзакция для получения списка слоёв
            using (Transaction tr = db.TransactionManager.StartTransaction())
            {
                PromptSelectionResult selectionRes = ed.SelectAll(sf);
                LayerTable tblLayer = (LayerTable)tr.GetObject(db.LayerTableId, OpenMode.ForRead, false);
                ed.WriteMessage("\nСписок всех слоёв в чертеже");
                foreach (ObjectId ent in tblLayer)
                {
                    LayerTableRecord entLayer = (LayerTableRecord)tr.GetObject(ent, OpenMode.ForRead);
                    ed.WriteMessage("\nИмя слоя : " + entLayer.Name);
                }

                // ищем совпадения слоёв 3Д полилиний по содержимому текстбокса 16-08-2024
                

                    if (selectionRes.Status == PromptStatus.OK)
                {
                    // добавляем id обьекта в список

                    // подсвечиваем выбранные обьекты
                    SelectionSet ss1 = SelectionSet.FromObjectIds(listSaveObject.ToArray());
                    ed.SetImpliedSelection(ss1);
                    return new ObjectIdCollection(selectionRes.Value.GetObjectIds());
                }
                else
                {
                    return new ObjectIdCollection();
                }

            }
        }
    }
}
using DevExpress.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;

namespace Gas_Monitoring.Models
{
    public class ResultSetting : BindableBase
    {
        private string name_kor;
        private string name_eng;
        private string rect_fill;
        private string rect_stk;
        private string img_src;
        private string label_forg;
        private string max_forg;
        private string max;

        public string Rect_fill { get => rect_fill; set => rect_fill = value; }
        public string Rect_stk { get => rect_stk; set => rect_stk = value; }
        public string Img_src { get => img_src; set => img_src = value; }
        public string Label_forg { get => label_forg; set => label_forg = value; }
        public string Name_kor { get => name_kor; set => name_kor = value; }
        public string Name_eng { get => name_eng; set => name_eng = value; }
        public string Max { get => max; set => max = value; }
        public string Max_forg { get => max_forg; set => max_forg = value; }

        public ResultSetting()
        {
            Init();
        }
        public ResultSetting(string kor=null, string eng= null, string fill= null, string stk= null, string src=null, string forg= null, string max_forg=null,string max =null)
        {
            Name_kor = kor;
            Name_eng = eng;
            Rect_fill = fill;
            Rect_stk = stk;
            Img_src = src;
            Label_forg = forg;
            Max_forg = max_forg;
            Max = max;
        }
        public void Init()
        {
            Name_kor = null;
            Name_eng = null;
            Rect_fill = null;
            Rect_stk = null;
            Img_src = null;
            Label_forg = null;
            Max_forg = null;
            Max = null;
        }
        public void Set(string kor, string eng, string fill, string stk, string src, string forg, string max_forg, string max)
        {
            Name_kor = kor;
            Name_eng = eng;
            Rect_fill = fill;
            Rect_stk = stk;
            Img_src = src;
            Label_forg = forg;
            Max_forg = max_forg;
            Max = max;
        }
    }
}

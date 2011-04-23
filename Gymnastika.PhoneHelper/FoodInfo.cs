using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Gymnastika.PhoneHelper
{
    [DataContract(Namespace = "")]
    public class FoodInfo
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public double Protein { get; set; }
        [DataMember]
        public double Carbohydrate { get; set; }
        [DataMember]
        public double Fat { get; set; }
        [DataMember]
        public double Calories { get; set; }
        [DataMember]
        public string Barcode { get; set; }
        public FoodInfo(Food food)
        {
            Name = food.Name;
            Barcode = food.Barcode;
            if (food.Protein != null)
                this.Protein = food.Protein.Value;
            if (food.Carbohydrate != null)
                this.Carbohydrate = food.Carbohydrate.Value;
            if (food.Fat != null)
                this.Fat = food.Fat.Value;
            if (food.Calories != null)
                this.Calories = food.Calories.Value;

        }
    }
}

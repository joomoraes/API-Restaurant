using System;

namespace Api.Application.Domain.Enums
{
    public enum EKitchen
    {
        Brazilian = 1,
        Italian = 2,
        Arabic = 3, 
        Japanese = 4,
        FastFood = 5
    }

    public static class EKitchenHelper
    {
        public static EKitchen ParseInt(int value)
        {
            if (Enum.TryParse(value.ToString(), out EKitchen kitchen))
                return kitchen;

            throw new ArgumentOutOfRangeException("kitchen");
        }

    }
}

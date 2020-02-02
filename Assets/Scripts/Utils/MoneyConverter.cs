using System;

public static class MoneyConverter {
    public static String IntToMoney(int money) {
        return FloatToMoney((float) money);
    }
    
    public static String FloatToMoney(float money) {
        var str = String.Format("{0:C}", money);
        return str.Substring(0, str.Length - 3);
    }
}
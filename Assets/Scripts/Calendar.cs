using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Calendar
{
    public float decimalTime;
    public Date CurrentDate { get; private set; }

    private Year year;
    private Month month;
    private Day day;

    [SerializeField]
    private int currentYear;
    [SerializeField]
    private int currentMonth;
    [SerializeField]
    private int currentDay;

    public Calendar()
    {
        decimalTime = 0f;

        currentYear = 2020;
        currentMonth = 1;
        currentDay = 1;

        year = new Year(currentYear);
        month = year.Months[currentMonth];
        day = year.Months[currentMonth].Days[currentDay]; // Calendar starts on a Sunday
        CurrentDate = new Date(day, month, year);
    }

    public void NextDay()
    {
        currentDay += 1;

        if (currentDay > month.NumOfDays)
        {
            currentMonth += 1;
            currentDay = 1;

            if (currentMonth > year.MonthsNum())
            {
                currentMonth = 1;
                currentYear += 1;

                year = new Year(year);
            }
        }

        month = year.Months[currentMonth];
        day = year.Months[currentMonth].Days[currentDay]; // Calendar starts on a Sunday
        CurrentDate = new Date(day, month, year);
    }
}

public enum DayName { NONE, Sunday, Monday, Tuesday, Wednesday, Thursday, Friday, Saturday }
public struct Day
{
    public readonly DayName DayOfWeek;
    public readonly int DayOfMonth;

    public Day(DayName name)
    {
        DayOfWeek = name;

        if (DayOfWeek == DayName.NONE)
            DayOfMonth = 0;
        else
            DayOfMonth = 1;
    }

    public Day(DayName name, int dayOfMonth)
    {
        DayOfWeek = name;

        if (DayOfWeek == DayName.NONE)
            DayOfMonth = 0;
        else
            DayOfMonth = dayOfMonth;
    }
}

public enum MonthName { NONE, January, February, March, April, May, June, July, August, September, October, November, December }
public struct Month
{
    public readonly MonthName MonthOfYear;
    public readonly int NumOfDays;
    public readonly Day[] Days;

    public Month(MonthName month)
    {
        if (month == MonthName.NONE)
        {
            MonthOfYear = month;
            NumOfDays = -1;
            Days = new Day[1];
        }
        else
        {
            MonthOfYear = month;
            DayName dayToStartOn = DayName.Sunday;


            switch (MonthOfYear)
            {
                case MonthName.January:
                    NumOfDays = 31;
                    break;
                case MonthName.February:
                    NumOfDays = 28;
                    break;
                case MonthName.March:
                    NumOfDays = 31;
                    break;
                case MonthName.April:
                    NumOfDays = 30;
                    break;
                case MonthName.May:
                    NumOfDays = 31;
                    break;
                case MonthName.June:
                    NumOfDays = 30;
                    break;
                case MonthName.July:
                    NumOfDays = 31;
                    break;
                case MonthName.August:
                    NumOfDays = 31;
                    break;
                case MonthName.September:
                    NumOfDays = 30;
                    break;
                case MonthName.October:
                    NumOfDays = 31;
                    break;
                case MonthName.November:
                    NumOfDays = 30;
                    break;
                case MonthName.December:
                    NumOfDays = 31;
                    break;
                default:
                    NumOfDays = -1;
                    break;
            }

            Days = new Day[NumOfDays + 1];

            DayName[] indexedDays = new DayName[8];
            indexedDays[0] = DayName.NONE;
            indexedDays[1] = DayName.Sunday;
            indexedDays[2] = DayName.Monday;
            indexedDays[3] = DayName.Tuesday;
            indexedDays[4] = DayName.Wednesday;
            indexedDays[5] = DayName.Thursday;
            indexedDays[6] = DayName.Friday;
            indexedDays[7] = DayName.Saturday;

            Days[0] = new Day(DayName.NONE);
            //for (int i = 1; i < Days.Length; i++)
            int dayShift = (int)dayToStartOn;
            for (int i = dayShift; i < Days.Length; i++)
            {
                if (i <= 7)
                {
                    Days[i] = new Day(indexedDays[i], i - dayShift);
                }
                else
                {
                    int newIndex = (i % 7);
                    if (newIndex == 0)
                        newIndex = 7;

                    Days[i] = new Day(indexedDays[newIndex], i - dayShift);
                }

            }
        } // Defaults to Month starting on a Sunday
    }

    public Month(MonthName month, int days, DayName dayLastMonthEndedOn)
    {
        MonthOfYear = month;

        NumOfDays = days;

        Days = new Day[NumOfDays + 1];

        DayName[] indexedDays = new DayName[8];
        indexedDays[0] = DayName.NONE;
        indexedDays[1] = DayName.Sunday;
        indexedDays[2] = DayName.Monday;
        indexedDays[3] = DayName.Tuesday;
        indexedDays[4] = DayName.Wednesday;
        indexedDays[5] = DayName.Thursday;
        indexedDays[6] = DayName.Friday;
        indexedDays[7] = DayName.Saturday;

        Days[0] = new Day(DayName.NONE);
        //for (int i = 1; i < Days.Length; i++)
        int dayShift = (int)dayLastMonthEndedOn + 1;
        for (int i = dayShift; i < Days.Length; i++)
        {
            if (i <= 7)
            {
                Days[i] = new Day(indexedDays[i], i - dayShift);
            }
            else
            {
                int newIndex = (i % 7);
                if (newIndex == 0)
                    newIndex = 7;

                Days[i] = new Day(indexedDays[newIndex], i - dayShift);
            }

        }
    }

    public Month(MonthName month, DayName dayLastMonthEndedOn)
    {
        MonthOfYear = month;

        switch (MonthOfYear)
        {
            case MonthName.January:
                //NumOfDays = 31;
                this = new Month(month, 31, dayLastMonthEndedOn);
                break;
            case MonthName.February:
                //NumOfDays = 28;
                this = new Month(month, 28, dayLastMonthEndedOn);
                break;
            case MonthName.March:
                //NumOfDays = 31;
                this = new Month(month, 31, dayLastMonthEndedOn);
                break;
            case MonthName.April:
                //NumOfDays = 30;
                this = new Month(month, 30, dayLastMonthEndedOn);
                break;
            case MonthName.May:
                //NumOfDays = 31;
                this = new Month(month, 31, dayLastMonthEndedOn);
                break;
            case MonthName.June:
                //NumOfDays = 30;
                this = new Month(month, 30, dayLastMonthEndedOn);
                break;
            case MonthName.July:
                //NumOfDays = 31;
                this = new Month(month, 31, dayLastMonthEndedOn);
                break;
            case MonthName.August:
                //NumOfDays = 31;
                this = new Month(month, 31, dayLastMonthEndedOn);
                break;
            case MonthName.September:
                //NumOfDays = 30;
                this = new Month(month, 30, dayLastMonthEndedOn);
                break;
            case MonthName.October:
                //NumOfDays = 31;
                this = new Month(month, 31, dayLastMonthEndedOn);
                break;
            case MonthName.November:
                //NumOfDays = 30;
                this = new Month(month, 30, dayLastMonthEndedOn);
                break;
            case MonthName.December:
                //NumOfDays = 31;
                this = new Month(month, 31, dayLastMonthEndedOn);
                break;
            default:
                //NumOfDays = -1;
                this = new Month(month);
                break;
        }

        #region oldcalc
        /*
        Days = new Day[NumOfDays + 1];

        DayName[] indexedDays = new DayName[8];
        indexedDays[0] = DayName.NONE;
        indexedDays[1] = DayName.Sunday;
        indexedDays[2] = DayName.Monday;
        indexedDays[3] = DayName.Tuesday;
        indexedDays[4] = DayName.Wednesday;
        indexedDays[5] = DayName.Thursday;
        indexedDays[6] = DayName.Friday;
        indexedDays[7] = DayName.Saturday;

        Days[0] = new Day(DayName.NONE);
        //for (int i = 1; i < Days.Length; i++)
        int dayShift = (int)dayLastMonthEndedOn + 1;
        for (int i = dayShift; i < Days.Length; i++)
        {
            if (i <= 7)
            {
                Days[i] = new Day(indexedDays[i], i - dayShift);
            }
            else
            {
                int newIndex = (i % 7);
                if (newIndex == 0)
                    newIndex = 7;

                Days[i] = new Day(indexedDays[newIndex], i - dayShift);
            }

        }
        */
        #endregion
    }
}

public struct Year
{
    public readonly int Number;
    public const int monthsNum = 12;
    public readonly Month[] Months;

    public Year(int number)
    {
        Number = number;

        Months = new Month[monthsNum + 1];
        Months[0] = new Month(MonthName.NONE);
        Months[1] = new Month(MonthName.January);
        Months[2] = new Month(MonthName.February, Number % 4 == 0 ? 29 : 28, Months[1].Days[Months[1].NumOfDays].DayOfWeek);
        Months[3] = new Month(MonthName.March, Months[2].Days[Months[2].NumOfDays].DayOfWeek);
        Months[4] = new Month(MonthName.April, Months[3].Days[Months[3].NumOfDays].DayOfWeek);
        Months[5] = new Month(MonthName.May, Months[4].Days[Months[4].NumOfDays].DayOfWeek);
        Months[6] = new Month(MonthName.June, Months[5].Days[Months[5].NumOfDays].DayOfWeek);
        Months[7] = new Month(MonthName.July, Months[6].Days[Months[6].NumOfDays].DayOfWeek);
        Months[8] = new Month(MonthName.August, Months[7].Days[Months[7].NumOfDays].DayOfWeek);
        Months[9] = new Month(MonthName.September, Months[8].Days[Months[8].NumOfDays].DayOfWeek);
        Months[10] = new Month(MonthName.October, Months[9].Days[Months[9].NumOfDays].DayOfWeek);
        Months[11] = new Month(MonthName.November, Months[10].Days[Months[10].NumOfDays].DayOfWeek);
        Months[12] = new Month(MonthName.December, Months[11].Days[Months[11].NumOfDays].DayOfWeek);
    }

    public Year(Year previousYear)
    {

        Number = previousYear.Number;

        Months = new Month[monthsNum + 1];
        Months[0] = new Month(MonthName.NONE);
        Months[1] = new Month(MonthName.January, previousYear.Months[12].Days[Months[12].NumOfDays].DayOfWeek);
        Months[2] = new Month(MonthName.February, Number % 4 == 0 ? 29 : 28, Months[1].Days[Months[1].NumOfDays].DayOfWeek);
        Months[3] = new Month(MonthName.March, Months[2].Days[Months[2].NumOfDays].DayOfWeek);
        Months[4] = new Month(MonthName.April, Months[3].Days[Months[3].NumOfDays].DayOfWeek);
        Months[5] = new Month(MonthName.May, Months[4].Days[Months[4].NumOfDays].DayOfWeek);
        Months[6] = new Month(MonthName.June, Months[5].Days[Months[5].NumOfDays].DayOfWeek);
        Months[7] = new Month(MonthName.July, Months[6].Days[Months[6].NumOfDays].DayOfWeek);
        Months[8] = new Month(MonthName.August, Months[7].Days[Months[7].NumOfDays].DayOfWeek);
        Months[9] = new Month(MonthName.September, Months[8].Days[Months[8].NumOfDays].DayOfWeek);
        Months[10] = new Month(MonthName.October, Months[9].Days[Months[9].NumOfDays].DayOfWeek);
        Months[11] = new Month(MonthName.November, Months[10].Days[Months[10].NumOfDays].DayOfWeek);
        Months[12] = new Month(MonthName.December, Months[11].Days[Months[11].NumOfDays].DayOfWeek);
    }

    public int MonthsNum()
    {
        return monthsNum;
    }
}

public struct Date
{
    public readonly Day Day;
    public readonly Month Month;
    public readonly Year Year;

    public Date(Day day, Month month, Year year)
    {
        Day = day;
        Month = month;
        Year = year;
    }
}

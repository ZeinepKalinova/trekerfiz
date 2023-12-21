using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<Activity> activities = new List<Activity>();
    static List<Achievement> achievements = new List<Achievement>();

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("1. Добавить активность");
            Console.WriteLine("2. Просмотреть историю активностей");
            Console.WriteLine("3. Посмотреть статистику");
            Console.WriteLine("4. Установить цель");
            Console.WriteLine("5. Посмотреть достижения");
            Console.WriteLine("6. Выход");

            Console.Write("Выберите действие (1-6): ");
            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    AddActivity();
                    break;
                case "2":
                    ViewHistory();
                    break;
                case "3":
                    ViewStatistics();
                    break;
                case "4":
                    SetGoal();
                    break;
                case "5":
                    ViewAchievements();
                    break;
                case "6":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Некорректный ввод. Пожалуйста, выберите действие от 1 до 6.");
                    break;
            }
        }
    }

    static void AddActivity()
    {
        Console.Write("Введите тип активности (например, ходьба, бег, велосипед): ");
        string type = Console.ReadLine();

        Console.Write("Введите продолжительность активности в минутах: ");
        if (int.TryParse(Console.ReadLine(), out int duration))
        {
            DateTime date = DateTime.Now;
            Activity newActivity = new Activity(date, type, duration);
            activities.Add(newActivity);
            Console.WriteLine("Активность добавлена успешно.");

            // Проверка достижений
            CheckAchievements();

            // Предложение рекомендаций
            RecommendActivity();
        }
        else
        {
            Console.WriteLine("Некорректный ввод для продолжительности. Введите число.");
        }
    }

    static void ViewHistory()
    {
        Console.WriteLine("История активностей:");
        foreach (var activity in activities.OrderByDescending(a => a.Date))
        {
            Console.WriteLine($"{activity.Date}: {activity.Type} - {activity.Duration} минут");
        }
    }

    static void ViewStatistics()
    {
        Console.Write("Введите период для просмотра статистики (например, 7 для последней недели): ");
        if (int.TryParse(Console.ReadLine(), out int period))
        {
            DateTime startDate = DateTime.Now.AddDays(-period);
            var filteredActivities = activities.Where(a => a.Date >= startDate);

            Console.WriteLine($"Статистика за последние {period} дней:");
            foreach (var type in filteredActivities.Select(a => a.Type).Distinct())
            {
                int totalDuration = filteredActivities.Where(a => a.Type == type).Sum(a => a.Duration);
                Console.WriteLine($"{type}: {totalDuration} минут");
            }
        }
        else
        {
            Console.WriteLine("Некорректный ввод для периода. Введите число.");
        }
    }

    static void SetGoal()
    {
        Console.Write("Введите тип активности для установки цели: ");
        string type = Console.ReadLine();

        Console.Write($"Введите цель по продолжительности {type} в минутах: ");
        if (int.TryParse(Console.ReadLine(), out int goal))
        {
            int totalDuration = activities.Where(a => a.Type == type).Sum(a => a.Duration);
            Console.WriteLine($"Общая продолжительность {type} за все время: {totalDuration} минут");

            if (totalDuration >= goal)
            {
                Console.WriteLine("Цель достигнута!");
            }
            else
            {
                Console.WriteLine($"Достигнутая продолжительность {type} меньше цели на {goal - totalDuration} минут");
            }
        }
        else
        {
            Console.WriteLine("Некорректный ввод для цели. Введите число.");
        }
    }

    static void ViewAchievements()
    {
        Console.WriteLine("Доступные достижения:");
        foreach (var achievement in achievements)
        {
            Console.WriteLine(achievement.Name);
        }
    }

    static void CheckAchievements()
    {
        // Пример достижения: Пройдено 100 км пешком
        int totalWalkingDistance = activities.Where(a => a.Type == "ходьба").Sum(a => a.Duration);
        if (totalWalkingDistance >= 100 * 60) // Переводим в минуты
        {
            UnlockAchievement("Пройдено 100 км пешком");
        }

        // Другие проверки для достижений
    }

    static void UnlockAchievement(string achievementName)
    {
        if (!achievements.Any(a => a.Name == achievementName))
        {
            Achievement newAchievement = new Achievement(achievementName);
            achievements.Add(newAchievement);
            Console.WriteLine($"Достигнуто достижение: {achievementName}");
        }
    }

    static void RecommendActivity()
    {
        // Пример рекомендации: Если пользователь меньше 30 минут ходил в течение дня
        int totalWalkingDuration = activities.Where(a => a.Type == "ходьба").Sum(a => a.Duration);
        if (totalWalkingDuration < 30)
        {
            Console.WriteLine("Рекомендуем увеличить продолжительность ходьбы для поддержания здоровья.");
        }

        // Другие проверки и рекомендации
    }
}

class Activity
{
    public DateTime Date { get; }
    public string Type { get; }
    public int Duration { get; }

    public Activity(DateTime date, string type, int duration)
    {
        Date = date;
        Type = type;
        Duration = duration;
    }
}

class Achievement
{
    public string Name { get; }

    public Achievement(string name)
    {
        Name = name;
    }
}

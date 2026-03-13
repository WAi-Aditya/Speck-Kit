namespace ITP.Api.Infrastructure.Configuration;

/// <summary>
/// Configuration options for the Skill Matrix module: Azure Blob Storage (certifications) and notification scheduler.
/// Bind from "SkillMatrix" configuration section (appsettings.json or environment).
/// </summary>
public class SkillMatrixOptions
{
    public const string SectionName = "SkillMatrix";

    /// <summary>
    /// Azure Blob Storage connection string for certification file uploads.
    /// </summary>
    public string AzureBlobConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// Container name for storing certification documents.
    /// </summary>
    public string CertificationsContainerName { get; set; } = "certifications";

    /// <summary>
    /// Whether to create the container on startup if it does not exist.
    /// </summary>
    public bool CreateContainerIfNotExists { get; set; } = true;

    /// <summary>
    /// Notification scheduler / reminder settings.
    /// </summary>
    public NotificationSchedulerOptions Notifications { get; set; } = new();
}

/// <summary>
/// Options for the notification reminder scheduler (e.g. Hangfire cron or hosted service).
/// </summary>
public class NotificationSchedulerOptions
{
    /// <summary>
    /// Cron expression for running the reminder job (e.g. "0 9 * * 1-5" for 9 AM weekdays).
    /// </summary>
    public string ReminderCron { get; set; } = "0 9 * * 1-5";

    /// <summary>
    /// Whether the reminder job is enabled.
    /// </summary>
    public bool ReminderEnabled { get; set; } = true;

    /// <summary>
    /// Number of days before certification expiry to send reminder.
    /// </summary>
    public int CertificationExpiryReminderDays { get; set; } = 30;

    /// <summary>
    /// Number of days before assessment due date to send reminder.
    /// </summary>
    public int AssessmentDueReminderDays { get; set; } = 7;
}

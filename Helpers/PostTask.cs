using System;

namespace WinFormsApp1.Helpers
{
    public record PostTask(
        string GroupUrl,
        string Content,
        string DatePost,       // MM/DD/YYYY
        string TimePost,       // HH:MM AM/PM
        string ImageFolderPath
    );
}

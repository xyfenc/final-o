namespace CPMTools;

public static class InputDialog
{
    public static string? Ask(string prompt, string def = "")
    {
        using var form = new Form
        {
            Width = 380,
            Height = 160,
            Text = "Input Required",
            StartPosition = FormStartPosition.CenterParent,
            FormBorderStyle = FormBorderStyle.FixedDialog,
            MaximizeBox = false,
            MinimizeBox = false
        };

        var label = new Label
        {
            Text = prompt,
            AutoSize = true,
            Location = new Point(12, 16)
        };

        var textbox = new TextBox
        {
            Text = def,
            Width = 330,
            Location = new Point(12, 40)
        };

        var ok = new Button
        {
            Text = "OK",
            DialogResult = DialogResult.OK,
            Width = 90,
            Location = new Point(160, 82)
        };

        var cancel = new Button
        {
            Text = "Cancel",
            DialogResult = DialogResult.Cancel,
            Width = 90,
            Location = new Point(260, 82)
        };

        form.Controls.Add(label);
        form.Controls.Add(textbox);
        form.Controls.Add(ok);
        form.Controls.Add(cancel);

        form.AcceptButton = ok;
        form.CancelButton = cancel;

        var result = form.ShowDialog();
        return result == DialogResult.OK ? textbox.Text : null;
    }
}

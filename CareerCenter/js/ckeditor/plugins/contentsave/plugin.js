CKEDITOR.plugins.add('contentsave',
    {
        init: function (editor) {
            var pluginName = 'contentsave';
            editor.ui.addButton('contentsave',
                {
                    label: 'Save',
                    command: 'cmdSaveContent',
                    icon: CKEDITOR.plugins.getPath('contentsave') + 'logo.png'
                });
            var cmd = editor.addCommand('cmdSaveContent', { exec: saveContent });
        }
    });
function saveContent(e) {
    ckContentSave();
}
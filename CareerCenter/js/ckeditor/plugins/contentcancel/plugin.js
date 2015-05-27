CKEDITOR.plugins.add('contentcancel',
    {
        init: function (editor) {
            var pluginName = 'contentcancel';
            editor.ui.addButton('contentcancel',
                {
                    label: 'Cancel',
                    command: 'cmdCancelContent',
                    icon: CKEDITOR.plugins.getPath('contentcancel') + 'logo.png'
                });
            var cmd = editor.addCommand('cmdCancelContent', { exec: cancelContent });
        }
    });
function cancelContent(e) {
    ckContentCancel()
}
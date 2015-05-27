/**
 * @license Copyright (c) 2003-2015, CKSource - Frederico Knabben. All rights reserved.
 * For licensing, see LICENSE.md or http://ckeditor.com/license
 */

//CKEDITOR.editorConfig = function( config ) {
//	// Define changes to default configuration here. For example:
//	// config.language = 'fr';
//	// config.uiColor = '#AADC6E';
//};


CKEDITOR.editorConfig = function (config) {
    config.extraPlugins = 'contentsave,contentcancel';
    config.toolbar = 'MyToolbar';

    config.toolbar_MyToolbar =
    [
        ['contentsave', 'contentcancel', '-', 'Undo', 'Redo', '-', 'Find', 'Replace'],  //'Cut', 'Copy', 'Paste', 'PasteText', 'PasteFromWord', '-',
        ['Scayt', '-', 'oembed', 'leaflet', 'Image', 'Table', 'HorizontalRule', 'Smiley', 'Symbol', '-', 'Link', 'Unlink', 'Anchor', '-', 'ShowBlocks', 'Sourcedialog' ],
        '/',
        ['Format', 'Font', 'FontSize'],
        ['TextColor', 'BGColor'],
        ['Bold', 'Italic', 'Underline', 'Strike', 'Subscript', 'Superscript', '-', 'NumberedList', 'BulletedList', 'Outdent', 'Indent', '-', 'Blockquote', 'CreateDiv'],
        ['JustifyLeft', 'JustifyCenter', 'JustifyRight', 'JustifyBlock']
    ];

    //config.scayt_autoStartup = true;
};
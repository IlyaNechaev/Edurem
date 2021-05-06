var editor = null;

function getEditor(language) {
  $(document).ready(function () {
    if (editor == null) {
      editor = CodeMirror.fromTextArea
        (document.getElementById('editor'), {
          mode: language,
          autoRefresh: true,
          lineNumbers: true
        });
      editor.on('change', function (cMirror) {
        // get value right from instance
        DotNet.invokeMethodAsync('Edurem', 'CodeMirrorValueChanged', getCode());
      });
    }
  });
}

function setCode(code) {
  editor.getDoc().setValue(code);
  setTimeout(function () {
    editor.refresh();
  }, 100);
}

function getCode() {
  return editor.getValue();
}



function setLanguage(language) {
  editor.setOption("mode", language);
}
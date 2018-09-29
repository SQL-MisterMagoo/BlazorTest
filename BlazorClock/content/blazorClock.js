// This file is to show how a library package may provide JavaScript interop features
// wrapped in a .NET API

window.blazorClock = {
    setCssProperty: function (id, property, value) {
        document.querySelector(id).style.setProperty(property, value);
        return true;
  }
};

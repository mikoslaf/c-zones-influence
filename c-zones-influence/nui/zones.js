const zones = {
  158:"Richman",
  77:"GWC & Golovng Society",
  119:"Morningwood",
  111:"Rockford Hills",
  181:"Burton",
  153:"West Vinewood",
  123:"Hawick",
  147:"Downtown Vinewood",
  129:"Alta",
  21:"Vinewood Racetrack",
  63:"East Vinewood",
  207:"Mirror Park",
  162:"La Mesa",
  171:"Murieta Heights",
  150:"Cypress Flats",
  144:"Terminal",
  79:"Elysian Island",
  188:"Banning",
  137:"Rancho",
  166:"Davis",
  //21:"Maze Bank Arena",
  106:"Chamberlain Hills",
  202:"Strawberry",
  138:"Mission Row",
  212:"Textile City",
  95:"Pillbox Hill",
  178:"Downtown",
  146:"Vinewood",
  198:"Little Seoul",
  182:"Richard Majestic",
  115:"Del Perro",
  134:"Del Peroo Beach",
  208:"Vespucci Canals",
  216:"Vespucci Beach",
  204:"La Puerta",
  160:"Los Santos International Airport"
}

window.addEventListener("message", function (event) {   
    if(event.data.action == "check") {
     search_map(event.data.x, event.data.y, event.data.v)
    } else if (event.data.action == "start") {
      search_map(event.data.x, event.data.y, event.data.v, true)
    }
  });

/*
lewo = -3428 | -2234
prawo = 3046 | 1881
sum = 6474 | 4115

góra = 8804 | 460
dół = -3390 | -3415
sum = 12194 | 3875

obraz 
x = 900 | 500
y = 1400 | 500
 */
function search_map(x,y,v = 0.001,praw = false) {
    console.log(x);
    console.log(y);
    const c = document.getElementById("myCanvas");
    const ctx = c.getContext("2d");
    const img = document.getElementById("scream");
    ctx.drawImage(img, 0, 0);
    const imgData = ctx.getImageData(0, 0, c.width, c.height);
    
    x = Math.round(((2234 + parseInt(x))/4116)* 500) * 4;
    y = Math.round((1 - ((3415 + parseInt(y))/3875)) * 500) * 500 * 4;

    $.post("https://c-zones-influence/c_influence", JSON.stringify({
      zone: 1,
      val: v
    }));
    if(praw) {
      console.log(imgData.data[x+y]);
    $("canvas").css("display","block");
    for (let i = x+y; i < (x + y + 800); i += 4) {
      imgData.data[i] = 247;
      imgData.data[i+1] = 255;
      imgData.data[i+2] = 0;
      imgData.data[i+3] = 255;
    }
    ctx.putImageData(imgData, 0, 0);

    setTimeout(() => {
      // for (let i = 0; i < imgData.data.length; i += 4) {
      //   //console.log(imgData.data[i] + ", " + imgData.data[i+1] + ", " + imgData.data[i +2] + ", " + imgData.data[i +3])
      //   imgData.data[i+3] = 0;
      // }
      // ctx.putImageData(imgData, 0, 0);
      $("canvas").css("display","none");
    }, 5000);

    }
}
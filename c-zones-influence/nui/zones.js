const zones = {
  1:"none",
  4:"Richman",
  11:"GWC & Golovng Society",
  18:"Morningwood",
  25:"Rockford Hills",
  32:"Burton",
  39:"West Vinewood",
  46:"Hawick",
  53:"Downtown Vinewood",
  60:"Alta",
  67:"Vinewood Racetrack",
  74:"East Vinewood",
  81:"Mirror Park",
  88:"La Mesa",
  95:"Murieta Heights",
  102:"Cypress Flats",
  109:"Terminal",
  116:"Elysian Island",
  123:"Banning",
  130:"Rancho",
  137:"Davis",
  //21:"Maze Bank Arena",
  144:"Chamberlain Hills",
  151:"Strawberry",
  158:"Mission Row",
  165:"Textile City",
  172:"Pillbox Hill",
  179:"Downtown",
  186:"Vinewood",
  193:"Little Seoul",
  200:"Richard Majestic",
  207:"Del Perro",
  214:"Del Peroo Beach",
  221:"Vespucci Canals",
  228:"Vespucci Beach",
  235:"La Puerta",
  249:"Los Santos International Airport"
}

window.addEventListener("message", function (event) {   
    if(event.data.action == "check") {
     search_map(event.data.x, event.data.y, event.data.v, event.data.n)
    } else if (event.data.action == "start") {
      search_map(event.data.x, event.data.y, event.data.v, "", true)
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
function search_map(lx,ly,v = 0.001,n,praw = false) {
    const c = document.getElementById("myCanvas");
    const ctx = c.getContext("2d");
    const img = document.getElementById("scream");
    ctx.drawImage(img, 0, 0);
    const imgData = ctx.getImageData(0, 0, c.width, c.height);
    
    const x = Math.round(((2234 + parseInt(lx))/4116)* c.width) * 4;
    const y = Math.round((1 - ((3415 + parseInt(ly))/3875)) * c.width) * c.height * 4;
    let result = imgData.data[x+y];
    if(result == 1 || result == undefined) return;
    console.log(result);
    console.log(zones[result]);

    if(zones[result] == undefined) 
    {
      let end = true;
      for(let i = result-3;i <= result+3; i++)
      {
        if(zones[i] != undefined) 
        {
          result = i;
          end = false;
          break;
        }
      }
      if(end) return;
    }

    if(n == "") {
      $.post("https://c-zones-influence/c_influence", JSON.stringify({
        zone: result,
        val: v,
        note: ""
      }));
    }
    else {
      $.post("https://c-zones-influence/c_influence", JSON.stringify({
        zone: result,
        val: v,
        note: n,
        name: zones[result]
      }));
    }
    
    if(praw) {
      console.log(result);
    $("canvas").css("display","block");
    for (let i = x+y+4; i < (x + y + 200); i += 4) {
      imgData.data[i] = 247;
      imgData.data[i+1] = 255;
      imgData.data[i+2] = 0;
      imgData.data[i+3] = 255;
    }
    console.log(imgData.data.length);
    let min = result - 3;
    let max =  result + 3;
    for (let i = 0; i < imgData.data.length; i += 4) {
      if(imgData.data[i] >= min && imgData.data[i] <= max) {
        imgData.data[i+3] = 55;
      }
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
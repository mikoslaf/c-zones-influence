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
let imgData = null;
let ctx = null;
window.addEventListener("message", function (event) {   
    if(event.data.action == "check") {
     search_map(event.data.x, event.data.y, event.data.v, event.data.n)
    } else if (event.data.action == "map") {
      //view_map(event.data.gang, event.data.zones)
      search_map2(event.data.x, event.data.y, event.data.v, event.data.n)
    }else if (event.data.action == "start") {
      search_map(event.data.x, event.data.y, event.data.v, "", event.data.gang)
    }
});

$( function() {
  $(".close-button").on("click", () => {
    $(".container").css("display","none");
    $.post("https://c-zones-influence/c_close", JSON.stringify({}));
    $(".notification-container").html("");
    create_canvas();
  });
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
async function create_canvas() {
  const c = document.getElementById("myCanvas");
  ctx = c.getContext("2d");
  const img = document.getElementById("scream");
  ctx.drawImage(img, 0, 0);
  imgData = ctx.getImageData(0, 0, c.width, c.height);
  return;
}

async function search_map2(x, y, v = 0.001, n, gang = "") {
  await create_canvas();
  let result = [];
  for (let i = 0; i < imgData.data.length; i += 4) {
    result.push(imgData.data[i])
  }
  console.log(result);
  $.post("https://c-zones-influence/c_influence2", JSON.stringify({
        tab: result,
        val: v
  }));
}

async function search_map(lx,ly,v = 0.001, n, gang = "") {
    if(imgData == null) {
      await create_canvas();
    }
    const x = Math.round(((2234 + parseInt(lx))/4116)* 500) * 4;
    const y = Math.round((1 - ((3415 + parseInt(ly))/3875)) * 500) * 500 * 4;
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
    
    if(gang != "") {
      $(".title").html("Gang: " + gang);
      $(".container").css("display","flex");
      for (let i = 0; i < imgData.data.length; i += 4) {
        if(imgData.data[i] == result) {
          imgData.data[i] = 255;
          imgData.data[i+1] = 0;
          imgData.data[i+2] = 0;
          imgData.data[i+3] = 155;
        } else {
          imgData.data[i+3] = 155;
          if(imgData.data[i] == 0) imgData.data[i+3] = 0; 
        }
      }
      ctx.putImageData(imgData, 0, 0);
    }
}

async function view_map(gang, zones, notes)
{
  zones = zones.replaceAll("\'","\"");
  zones  = '{"1": ["none","0"],"2": ["nda","0.8"],"4": ["vagos","0.6"],"193": ["inny","1"],"130": ["police","0.3"],"67": ["ballas","0.8"],"200": ["ballas","0.9"],"137": ["none","0"],"74": ["none","0"],"11": ["none","0"],"207": ["vagos","0.35"],"144": ["vagos","0.413"],"81": ["none","0"],"18": ["none","0"],"214": ["none","0"],"151": ["none","0"],"88": ["triads","0.5"],"25": ["vagos","0.01"],"221": ["triads","1"],"158": ["vagos","0.01"],"95": ["triads","0.75"],"32": ["none","0"],"228": ["none","0"],"165": ["none","0"],"102": ["none","0"],"39": ["vagos","0.9"],"235": ["vagos","1"],"172": ["vagos","0.01"],"109": ["none","0"],"46": ["none","0"],"179": ["none","0"],"116": ["none","0"],"53": ["none","0"],"249": ["none","0"],"186": ["none","0"],"123": ["none","0"],"60": ["none","0"]}';
  zones = JSON.parse(zones);
  if(imgData == null) {
    await create_canvas();
  }
  let not = true; 
  for (let i = 0; i < notes; i += 2) {
    not = false; 
    addNotification(notes[i], notes[i+1])
  }
  if(not)
  {
    addNotification("No notifications", "You do not have any notifications");
  }
  $.getJSON("config.json", function(gangs) {
    const map = imgData;
    for (let i = 0; i < map.data.length; i += 4) {
      if(zones[map.data[i].toString()] != undefined) {
        const zone = zones[map.data[i].toString()];
        if(gangs[zone[0]] == undefined) {
          map.data[i] = 169;
          map.data[i+1] = 169;
          map.data[i+2] = 169;
        } else {
          map.data[i] = gangs[zone[0]][0];
          map.data[i+1] = gangs[zone[0]][1];
          map.data[i+2] = gangs[zone[0]][2];
        }
        map.data[i+3] = parseFloat(zone[1]) * 0.7 * 255;
      }
      else {
        map.data[i+3] = 0;
      } 
    }
  ctx.putImageData(map, 0, 0);

  let cont = 0; 
  for (const [_, value] of Object.entries(zones)) {
    if(value[0] == gang && parseFloat(value[1]) > 0.8) cont++;
  }
  $(".title").html("Gang: " + gang);
  $(".description:first").html("Your gang has "+cont+" occupied zones.");
  $(".container").css("display","flex");
  });
}

function addNotification(title, description) {
  const notification = document.createElement("div");
  notification.classList.add("notification");

  const titleElement = document.createElement("h2");
  titleElement.classList.add("title-p");
  titleElement.textContent = title;

  const descriptionElement = document.createElement("p");
  descriptionElement.classList.add("description");
  descriptionElement.textContent = description;

  notification.appendChild(titleElement);
  notification.appendChild(descriptionElement);

  document.querySelector(".notification-container").appendChild(notification);
}
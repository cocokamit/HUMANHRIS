 
;( function( window ) {
	
	'use strict';

	function extend( a, b ) {
		for( var key in b ) { 
			if( b.hasOwnProperty( key ) ) {
				a[key] = b[key];
			}
		}
		return a;
	}

	function CBPFWTabs( el, options ) {
		this.el = el;
		this.options = extend( {}, this.options );
  		extend( this.options, options );
  		this._init();
	}

	CBPFWTabs.prototype.options = {
		start : 0
	};

	CBPFWTabs.prototype._init = function() {
		// tabs elems
		this.tabs = [].slice.call( this.el.querySelectorAll( '.nav > li' ) );
		
        // content items
		this.items = [].slice.call( this.el.querySelectorAll( '.tab-content > .tab-pane' ) );
		// current index
		this.current = -1;
		// show current content item
		this._show();
		// init events
		this._initEvents();
	};

	CBPFWTabs.prototype._initEvents = function() {
		var self = this;
		this.tabs.forEach( function( tab, idx ) {
			tab.addEventListener( 'click', function( ev ) {
				ev.preventDefault();
                 var x = document.getElementById('tab_index');
                x.value = idx;


               // self._show(idx);
			} );
		} );
	};

	CBPFWTabs.prototype._show = function( idx ) {
		if( this.current >= 0 ) {
			this.tabs[ this.current ].className = this.items[ this.current ].className = '';
		}
	 
        /*BUTYOK
        -add className [allower]
        */
		this.current =  document.getElementById("tab_index").value;
		this.tabs[ this.current ].className = 'allower active';
        this.tabs[ this.current ].innerHTML += "<div style='border:2px solid #fff;position:absolute; width:100%'></div>";
		this.items[ this.current ].className = 'tab-pane active';

	};

	// add to global namespace
	window.CBPFWTabs = CBPFWTabs;

})( window );
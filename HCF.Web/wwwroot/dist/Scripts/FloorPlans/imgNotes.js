; (function ($) {
    $.widget("wgm.imgNotes", {
        options: {
            zoom: 1,
            zoomStep: 0.2,
            canEdit: true,
            canDrop: true,
            vAll: "middle",
            hAll: "middle",
            onAdd: function (text, icon, status, duedate, floorassetid) {
                this.options.vAll = "bottom"; 
                this.options.hAll = "middle";
                var elem = $(document.createElement('div')).addClass("marker assets floorAsset point").append($('<p style="display:none;" class="marker-text2">' + (this.notes.length + 1) + '</p>'))
                    .prepend($('<img>', { src: icon, width: '100%', imagepath: icon })).attr("id", floorassetid).attr("title", text).attr("status", status).attr("duedate", duedate);
                $(elem).tooltip({
                    content: function () {
                        return $(elem).data("note").note;
                    },
                    show: true,
                    hide: { delay: 700 },
                    position: {
                        within: $(this.view),
                        collision: "flipfit"
                    }
                });
                return elem;
            },
            onEdit: function (ev, elem) {
                var $elem = $(elem);
                $('#NoteDialog').remove();
                editAssetPopUp($elem);

            },
            onShow: function (ev, elem) {
                var $elem = $(elem);
                $('#NoteDialog').remove();
                openShowPopUp($elem);
            }
        },

        _create: function () {
            var self = this;
            if (!this.element.is("img")) {
                $.error('imgNotes plugin can only be applied to img elements');
            }

            self.notes = [];
            self.noteCount = 0;
            self.img = self.element[0];
            var $img = $(self.img);

            $img.imgViewer({
                onClick: function (ev, imgv) {
                    debugger;
                    if (self.options.canDrop) {
                        ev.preventDefault();
                        var rpos = imgv.cursorToImg(ev.pageX, ev.pageY);
                        if (rpos) {
                            var dropItem = imgv.getDropItem();
                            var dropFloorAssetId = dropItem.attr('floorAssetId');
                            var assetNo = dropItem.attr("assetno");
                            var src = dropItem.attr('src');
                            var status = dropItem.attr('status');
                            var duedate = dropItem.attr('duedate');
                            if (dropFloorAssetId) {
                                var elem = self.addNote(rpos.x, rpos.y, assetNo, dropFloorAssetId, 0, src, status, duedate);
                                addAssetsToPlan(elem);                                
                            }
                        }
                    }
                },
                onUpdate: function () {
                    $.each(self.notes, function () {
                        self._updateMarkerPos(this);
                    });
                }
            });
        },

        destroy: function () {
            this.clear();
            $(this.img).imgViewer("destroy");
            $.Widget.prototype.destroy.call(this);
        },

        _setOption: function (key, value) {
            switch (key) {
                case 'vAll':
                    switch (value) {
                        case 'top': break;
                        case 'bottom': break;
                        default: value = 'middle';
                    }
                    break;
                case 'hAll':
                    switch (value) {
                        case 'left': break;
                        case 'right': break;
                        default: value = 'middle';
                    }
                    break;
            }
            var version = $.ui.version.split('.');
            if (version[0] > 1 || version[1] > 8) {
                this._super(key, value);
            } else {
                $.Widget.prototype._setOption.apply(this, arguments);
            }
            switch (key) {
                case 'zoom':
                    $(this.img).imgViewer("option", "zoom", value);
                    break;
                case 'zoomStep':
                    $(this.img).imgViewer("option", "zoomStep", value);
                    break;
            }
        },

        addNote: function (relx, rely, text, id, type, icon, status, duedate) {
            var self = this;
            this.noteCount++;
            var elem = this.options.onAdd.call(this, text, icon, status, duedate, id);
            var $elem = $(elem);
            $(this.img).imgViewer("addElem", elem);
            $elem.data("relx", relx).data("rely", rely).data("note", text).data("id", id).data("type", type).data("icon", icon).data("status", status).data("duedate", duedate);

            switch (this.options.vAll) {
                case "top": $elem.data("yOffset", 0); break;
                case "bottom": $elem.data("yOffset", $elem.height()); break;
                default: $elem.data("yOffset", Math.round($elem.height() / 2));
            }
            switch (this.options.hAll) {
                case "left": $elem.data("xOffset", 0); break;
                case "right": $elem.data("xOffset", $elem.width()); break;
                default: $elem.data("xOffset", Math.round($elem.width() / 2));
            }
            $elem.click(function (ev) {
                ev.preventDefault();
                // console.log(elem);
                if (self.options.canEdit) {
                    self._trigger("onEdit", ev, elem);
                } else {
                    self._trigger("onShow", ev, elem);
                }
            });
            $elem.on("remove", function () {
                self._delete(elem);
            });
            this._updateMarkerPos(elem);
            this.notes.push(elem);
            return elem;
        },

        count: function () {
            return this.noteCount;
        },

        _updateMarkerPos: function (elem) {
            var $elem = $(elem),
                $img = $(this.img);
            var pos = $img.imgViewer("imgToView", $elem.data("relx"), $elem.data("rely"));
            if (pos) {
                // console.log(pos);
                $elem.css({
                    left: (pos.x - $elem.data("xOffset")),
                    top: (pos.y - $elem.data("yOffset")),
                    position: "absolute"
                });
            }
        },

        _delete: function (elem) {
            this.notes = this.notes.filter(function (v) { return v !== elem; });
            $(elem).remove();
        },

        clear: function () {
            var self = this;
            $.each(self.notes, function () {
                var $elem = $(this);
                $elem.remove();
            });
            self.notes = [];
            self.noteCount = 0;
        },

        import: function (notes) {
            var self = this;
            $.each(notes, function () {
                self.addNote(this.x, this.y, this.note, this.id, this.type, this.icon, this.status, this.duedate);
            });
        },

        export: function () {
            var notes = [];
            $.each(this.notes, function () {
                var $elem = $(this);
                notes.push({
                    x: $elem.data("relx"),
                    y: $elem.data("rely"),
                    note: $elem.data("note"),
                    id: $elem.data("id"),
                    type: $elem.data("type"),
                    icon: $elem.data("icon"),
                    status: $elem.data("status"),
                    duedate: $elem.data("duedate")
                });
            });
            return notes;
        }
    });
})(jQuery);

// SPDX-License-Identifier: UNLICENSED
pragma solidity ^0.8.20;

import { Ownable } from "@openzeppelin/contracts/access/Ownable.sol";
import { IERC165 } from "@openzeppelin/contracts/utils/introspection/IERC165.sol";
import { IERC721 } from "@openzeppelin/contracts/token/ERC721/IERC721.sol";
import { ERC721Holder } from "@openzeppelin/contracts/token/ERC721/utils/ERC721Holder.sol";
import { IERC1155 } from "@openzeppelin/contracts/token/ERC1155/IERC1155.sol";
import { ERC1155Holder } from "@openzeppelin/contracts/token/ERC1155/utils/ERC1155Holder.sol";

contract ZMOContract is ERC1155Holder, ERC721Holder, Ownable {
    event CreateOffer(uint256 _offerId, OfferInfo _offerInfo);
    event UpdateOffer(uint256 _offerId, OfferInfo _offerInfo);
    event AcceptOffer(uint256 _offerId, OfferInfo _offerInfo);
    event ClaimOffer(uint256 _offerId, OfferInfo _offerInfo);

    bytes4 private constant _INTERFACE_ID_ERC721 = 0x80ac58cd;
    bytes4 private constant _INTERFACE_ID_ERC1155 = 0xd9b67a26;

    struct OfferInfo {
        address nftAddress;
        uint256 nftId;
        uint256 latestAmount;
        address buyer;
        address seller;
        uint256 offerAt;
    }

    uint256 public feeSystem;
    uint256 public feeSuccess;
    uint256 public timeExpire;
    mapping(address owner => uint256 fee) feeBalance;
    mapping(address seller => uint256 balance) sellerBalance;
    mapping(address buyer => uint256 remainAmount) buyerAmount;

    uint256 public offerId;
    mapping(uint256 offerId => OfferInfo offerInfo) offerIds;
    mapping(uint256 offerId => bool accepted) offerStatus;

    constructor(address _initialOwner) Ownable(_initialOwner) {
        timeExpire = 28 hours;
        feeSystem = 0;
        feeSuccess = 0;
    }

    function setTimeExpire(uint256 _expire) public onlyOwner {
        timeExpire = _expire;
    }

    function setFeeSystem(uint256 _feeSystem) public onlyOwner {
        feeSystem = _feeSystem;
    }

    function setFeeSuccess(uint256 _feeSuccess) public onlyOwner {
        feeSuccess = _feeSuccess;
    }

    function _calculateFee(uint256 _fee, uint256 _amount) internal pure returns (uint256) {
        return (_amount * _fee) / 100;
    }

    function createOffer(address _nftAddress, uint256 _nftId, address _seller) public payable {
        uint256 amount = msg.value;

        require(amount > 0, "Invalid amount");

        uint256 feeSystemValue = _calculateFee(feeSystem, amount);
        uint256 latestAmount = amount - feeSystemValue;
        feeBalance[owner()] += feeSystemValue;

        OfferInfo memory newOffer = OfferInfo({
            nftAddress: _nftAddress,
            nftId: _nftId,
            latestAmount: latestAmount,
            buyer: msg.sender,
            seller: _seller,
            offerAt: block.timestamp
        });

        ++offerId;
        offerStatus[offerId] = false;
        offerIds[offerId] = newOffer;
        emit CreateOffer(offerId, newOffer);
    }

    function updateOffer(uint256 _offerId) public payable {
        uint256 amount = msg.value;
        address sender = msg.sender;

        require(amount > 0, "Invalid amount");
        require(_offerId > 0 && _offerId <= offerId, "Invalid offer id");

        OfferInfo memory offer = offerIds[_offerId];
        require(offer.buyer == sender, "Invalid buyer");
        require(offerStatus[_offerId] == false, "Offer must not be accepted");
        require(amount > offer.latestAmount, "Invalid latest amount");

        buyerAmount[sender] += offer.latestAmount;

        uint256 feeSystemValue = _calculateFee(feeSystem, amount);
        uint256 latestAmount = amount - feeSystemValue;
        feeBalance[owner()] += feeSystemValue;
        offer.latestAmount = latestAmount;
        offer.offerAt = block.timestamp;
        emit UpdateOffer(_offerId, offer);
    }

    function getOffer(uint256 _offerId) public view returns (OfferInfo memory) {
        require(_offerId > 0 && _offerId <= offerId, "Invalid offer id");
        return offerIds[_offerId];
    }

    function getSellerBalance() public view returns (uint256) {
        return sellerBalance[msg.sender];
    }

    function getBuyerAmount() public view returns (uint256) {
        return buyerAmount[msg.sender];
    }

    function getFeeBalance() public view returns (uint256) {
        return feeBalance[owner()];
    }

    function acceptOffer(uint256 _offerId) public {
        address sender = msg.sender;

        require(_offerId > 0 && _offerId <= offerId, "Invalid offer id");

        OfferInfo memory offerInfo = offerIds[_offerId];

        require(offerStatus[_offerId] == false, "Offer must not be accepted");
        require(offerInfo.offerAt + timeExpire > block.timestamp, "Offer expire"); // Có nên check case này không?
        require(offerInfo.seller == sender, "Invalid seller");

        bool isERC721 = _isERC721(offerInfo.nftAddress);
        bool isERC1155 = _isERC1155(offerInfo.nftAddress);

        if (isERC721) {
            if (IERC721(offerInfo.nftAddress).ownerOf(offerInfo.nftId) != sender) {
                revert("Invalid Owner of Nft");
            }

            IERC721(offerInfo.nftAddress).safeTransferFrom(sender, address(this), offerInfo.nftId);
        } else if (isERC1155) {
            if (IERC1155(offerInfo.nftAddress).balanceOf(sender, offerInfo.nftId) == 0) {
                revert("Invalid number of Nft");
            }

            IERC1155(offerInfo.nftAddress).safeTransferFrom(sender, address(this), offerInfo.nftId, 1, "");
        } else {
            revert("Address is not Nft");
        }

        offerStatus[_offerId] = true;
        uint256 feeSuccessValue = _calculateFee(feeSuccess, offerInfo.latestAmount);
        uint256 latestAmount = offerInfo.latestAmount - feeSuccessValue;
        feeBalance[owner()] += feeSuccessValue;
        sellerBalance[sender] += latestAmount;
        emit AcceptOffer(_offerId, offerInfo);
    }

    function claimRemainAmount() public {
        address sender = msg.sender;
        uint256 remainAmount = buyerAmount[sender];

        require(remainAmount > 0, "Invalid remain amount");
        buyerAmount[sender] = 0;
        (bool success,) = payable(sender).call{ value: remainAmount }("");
        require(success, "Transfer failed.");
    }

    function claimNft(uint256 _offerId) public {
        address sender = msg.sender;
        OfferInfo memory offerInfo = offerIds[_offerId];

        require(_offerId > 0 && _offerId <= offerId, "Invalid offer id");
        require(
            offerStatus[_offerId] == true || (offerStatus[_offerId] == false && offerInfo.offerAt + timeExpire > block.timestamp),
            "Offer must not be claimed"
        );

        if (offerStatus[_offerId] == true) {
            bool isERC721 = _isERC721(offerInfo.nftAddress);

            if (isERC721) {
                if (IERC721(offerInfo.nftAddress).ownerOf(offerInfo.nftId) != address(this)) {
                    revert("Invalid Owner of Nft");
                }

                IERC721(offerInfo.nftAddress).transferFrom(address(this), sender, offerInfo.nftId);
            } else {
                if (IERC1155(offerInfo.nftAddress).balanceOf(address(this), offerInfo.nftId) == 0) {
                    revert("Invalid number of Nft");
                }

                IERC1155(offerInfo.nftAddress).safeTransferFrom(address(this), sender, offerInfo.nftId, 1, "");
            }
            emit ClaimOffer(_offerId, offerInfo);
        } else if (offerStatus[_offerId] == false && offerInfo.offerAt + timeExpire > block.timestamp) {
            require(offerInfo.latestAmount > 0, "Invalid amount claim");

            offerInfo.latestAmount = 0;
            (bool success,) = payable(offerInfo.buyer).call{ value: offerInfo.latestAmount }("");
            require(success, "Transfer failed.");
        } else {
            revert("Can't claim");
        }
    }

    function claimSellerBalance(uint256 _amount) public {
        address sender = msg.sender;

        require(_amount > 0, "Invalid amount claim");
        require(_amount <= sellerBalance[sender], "Invalid balance");

        sellerBalance[sender] -= _amount;
        (bool success,) = payable(sender).call{ value: _amount }("");
        require(success, "Transfer failed.");
    }

    function withDraw(uint256 _amount) public onlyOwner {
        // Có cần thiết amount không hay cho claim 1 phát vì amount này là phí hệ thống (nhỏ)
        require(_amount > 0, "Invalid amount claim");
        require(_amount <= feeBalance[owner()], "Invalid balance");

        feeBalance[owner()] -= _amount;
        (bool success,) = payable(owner()).call{ value: _amount }("");
        require(success, "Transfer failed.");
    }

    function _isERC721(address _nftAddress) internal view returns (bool) {
        return IERC165(_nftAddress).supportsInterface(_INTERFACE_ID_ERC721);
    }

    function _isERC1155(address _nftAddress) internal view returns (bool) {
        return IERC165(_nftAddress).supportsInterface(_INTERFACE_ID_ERC1155);
    }

    receive() external payable { }
}
